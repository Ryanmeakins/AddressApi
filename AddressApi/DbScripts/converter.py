from os import listdir
from os.path import isdir, join
from csv import reader as csv_reader
import json
import mysql.connector
import traceback

CSV_FILE_PATH = ""

STANDARD_COLUMNS = ["NUMBER", "STREET", "UNIT", "CITY", "DISTRICT", "REGION", "POSTCODE"]

DB_USER = ""
DB_NAME = ""
DB_PWD = ""
TABLE = ""

STOP_AFTER_ROW = 300
SKIP_AFTER_SUB = 15
SKIP_AFTER_DIR = 15


countries = []
country_details = {}

with open('./config.json') as json_file:
    data = json.load(json_file)
    print('data is:', data)
    print()
    DB_USER = data.get('db_user', "")
    DB_NAME = data.get('db_name', "")
    DB_PWD = data.get('db_password', "")
    TABLE = data.get('db_table', "")
    CSV_FILE_PATH = data.get('csv_file_path', "")
    STOP_AFTER_ROW = data.get('stop_after_row', 999999999)
    SKIP_AFTER_SUB = data.get('skip_after_csv', 999999999)
    SKIP_AFTER_DIR = data.get('skip_after_sub_dir', 999999999)

def columns_from_csv(file_path):
    list_of_column_names = []
    with open(file_path, encoding="utf8") as csv_file:
        reader = csv_reader(csv_file, delimiter = ',')
        for row in reader:
            list_of_column_names.append(row) 
            break
    
    return list_of_column_names

SQL_STATEMENT = "INSERT INTO country_data (country, region, district, city, "\
    "street, number, unit, postcode) VALUES (%s, %s, %s, %s, %s, %s, %s, %s)"

def update_db(file_path, country, district=None):
    with open(file_path, encoding="utf8") as csv_file:
        reader = csv_reader(csv_file, delimiter = ',')
        count = 0
        for row in reader:
            if count == 0:
                count += 1
                continue
            if row[6] == "" and district:
                row[6] = district
            values = (country, row[7], row[6], row[5], row[3], row[2], row[4], row[8],)

            try:
                cursor.execute(SQL_STATEMENT, values)
            except Exception:
                print("execute exception for row: {}".format(row))
                print("received exception at count {}: {}".format(count))
                traceback.print_exc()
                print()

            # print("exec_resp: ", str(exec_resp))
            count += 1
            try:
                cnx.commit()
            except Exception:
                print("commit execption for row: {}".format(row))
                print("received exception at count {}: {}".format(count))
                traceback.print_exc()
                print()
            
            if STOP_AFTER_ROW:
                if count > STOP_AFTER_ROW:
                    break

    try:
        cnx.commit()
    except Exception as e:
        print("received exception at end commit {}: {}".format(count))
        traceback.print_exc()
        print()


def setup_table():
    # DROP_DB_STATEMENT = "DROP DATABASE IF EXISTS {}".format(DB_NAME)
    # cursor.execute(DROP_DB_STATEMENT)
    # CREATE_DB_STATEMENT = "CREATE DATABASE {}".format(DB_NAME)
    # cursor.execute(CREATE_DB_STATEMENT)

    DROP_TABLE_STATEMENT = "DROP TABLE IF EXISTS {}".format(TABLE)
    cursor.execute(DROP_TABLE_STATEMENT)

    CREATE_TABLE =  """CREATE TABLE `country_data` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `country` varchar(45) CHARACTER SET 'utf8' NOT NULL,
  `region` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  `district` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  `city` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  `street` varchar(145) CHARACTER SET 'utf8' DEFAULT NULL,
  `number` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  `unit` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  `postcode` varchar(45) CHARACTER SET 'utf8' DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
)"""
    cursor.execute(CREATE_TABLE)
    cnx.commit()


try:
    cnx = mysql.connector.connect(user=DB_USER, host="localhost", password=DB_PWD, database=DB_NAME)
    cursor = cnx.cursor()
    setup_table()
    cnx.close()

    cnx = mysql.connector.connect(user=DB_USER, host="localhost", password=DB_PWD, database=DB_NAME)
    cursor = cnx.cursor()
except Exception as e:
    print("received error connecting to db: ", str(e))
    traceback.print_exc()
    exit()


country_dirs = listdir(CSV_FILE_PATH)
# print("files found:", country_dirs)
# print()
for country in country_dirs:
    if country == 'summary':
        continue
    country_path = join(CSV_FILE_PATH, country)
    if isdir(country_path):
        country_details[country] = {}
        # countries.append(country)
        files_inside_country = listdir(country_path)
        if len(files_inside_country) == 0:
            print("no files found inside country '{}'".format(country))
            continue
        country_dirs = [f for f in files_inside_country if isdir(join(country_path, f))]
        country_files = [f for f in files_inside_country if not isdir(join(country_path, f))]
        country_files = [f for f in country_files if f.endswith('csv')]
        has_summary = country_files.__contains__('countrywide.csv')
        print("Working on country: {}".format(country))
        # print("country '{}', has dirs: {} -- cvs: {} -- has_summary: {}".format(country, len(country_dirs), len(country_files), has_summary))
        if has_summary:
            print("  Country has 'countrywide.csv', adding those")
            # print("country '{}', has dirs: {} -- cvs: {}".format(country, len(country_dirs), len(country_files),))
            update_db(join(country_path, 'countrywide.csv'), country)
            # break
        else:
            print("country '{}', has dirs: {} -- cvs: {}".format(country, len(country_dirs), len(country_files),))
            if len(country_files) > 0:
                file_count_in_dir = 0
                for f in country_files:
                    print("  working on csv: {}".format(f))
                    update_db(join(country_path, f), country)
                    file_count_in_dir += 1
                    if file_count_in_dir > SKIP_AFTER_SUB:
                        break
            
            if len(country_dirs) > 0:
                DIR_COUNT = 0
                for dr in country_dirs:
                    country_sub_path = join(country_path, dr)
                    files_inside_country_dir = listdir(country_sub_path)
                    files_inside_country_dir = [f for f in files_inside_country_dir if f.endswith('csv')]

                    file_count_in_dir = 0
                    for f in files_inside_country_dir:
                        print("  working on dir '{}' csv: {}".format(dr, f))
                        update_db(join(country_sub_path, f), country, dr)
                        file_count_in_dir += 1
                        if file_count_in_dir > SKIP_AFTER_SUB:
                            break
                    
                    DIR_COUNT += 1
                    if DIR_COUNT > SKIP_AFTER_DIR:
                        break

        print()
        # if has_summary:
        #     update_db(join(country_path, 'countrywide.csv'), country)
        #     break
        # if not has_summary:
        #     print("country '{}', has dirs: {} -- cvs: {}".format(country, len(country_dirs), len(country_files),))
            # if isdir(join(country_path, country_files[0])):
            #     if country_files[0].isnumeric():
            #         country_details[country]['sub_region_type'] = 'district'
            #     else:
            #         country_details[country]['sub_region_type'] = 'region'
            #     country_details[country]['regions'] = country_files
            # else:
            #     file_path = join(country_path, country_files[0])
            #     cols = columns_from_csv(file_path)
            #     print("  for country {} columns inside csv '{}' are: {}".format(country, country_files[0], str(cols)))


