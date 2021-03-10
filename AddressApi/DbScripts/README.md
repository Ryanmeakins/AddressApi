# Database setup
1. Install mysql (no particular version, latest should be fine)
2. Create user account that provides all permissions.
    - Refer to this: https://www.digitalocean.com/community/tutorials/how-to-create-a-new-user-and-grant-permissions-in-mysql
      - Create with all perms for this project
    - This account can be used for the script
3. Create database by logging into the mysql shell: Ex:
    ```
    CREATE SCHEMA `cpsc-5200` DEFAULT CHARACTER SET utf8 ;
    ```
4. Fill the config.json file in this directory with the DB details above:
    - Lower the "stop_after_row", "stop_after_sub_dir", "skip_after_csv" if you don't want to load all the data
        ```
        {
            "csv_file_path": "/home/harsha/Desktop/from_one_drive/",
            "db_user": "harsha",
            "db_password": "my_password",
            "db_name": "country_database",
            "db_table": "country_data",
            "stop_after_row": 999999, // script will stop adding new rows from CSV into DB
            "skip_after_sub_dir": 100, // script will stop reading sub directories inside a country after crossing this many and move on to next country
            "skip_after_csv": 100 // script will reading new CSVs after reading this many CSVs from a given folder and move on to next country
        }
        ```
5. Setup python environment to execute
    - install python3 (any version should be fine)
    - install python virtual environment: 
      - Windows: https://docs.python.org/3/library/venv.html
      - Mac and Linux: https://packaging.python.org/guides/installing-using-pip-and-virtual-environments/
    - create virtual env (refer to above links)
    - source/enter the env
    - install the requirements: pip install -r requirements.txt
6. execute python script:
    ```
    (venv) group_project$ python3 converter.py 
    data is: {'csv_file_path': '/home/harsha/Desktop/from_one_drive/', 'db_user': 'harsha', 'db_password': 'passpass', 'db_name': 'country_database', 'db_table': 'country_data', 'stop_after_row': 300, 'skip_after_sub_dir': 15, 'skip_after_csv': 15}

    Working on country: ch
    Country has 'countrywide.csv', adding those

    Working on country: nc
    Country has 'countrywide.csv', adding those

    ...
    ```