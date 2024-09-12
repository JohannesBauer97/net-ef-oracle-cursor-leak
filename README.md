# EF-Oracle Cursor "Leak" Example
This is a simple example of how to reproduce the cursor issue with Oracle and Entity Framework.

## Steps to reproduce
1. Start Oracle database using docker: `docker run --name oracle -d -p 1521:1521 -p 5500:5500 -e "ORACLE_PWD=superSecret12345" --restart always container-registry.oracle.com/database/express:21.3.0-xe`
2. Run the `WebApplication1` project
3. Open SQL Developer and connect to the Oracle database
4. Run the following query to see the number of open cursors for each user:
```sql
select
    user_name,
    count(*) as "OPEN CURSORS"
from
    v$open_cursor
group by
    user_name;
```
5. Run the following python script to simulate traffic to the application:
```python
import requests
import concurrent.futures

# Define the URL
url = 'http://localhost:5160/api/API'

# Function to perform a single GET request
def make_request(index):
    try:
        response = requests.get(url, verify=False)
        print(f"Request {index+1}: Status Code: {response.status_code}")
        return response.status_code
    except requests.exceptions.RequestException as e:
        print(f"Request {index+1} failed: {e}")
        return None

with concurrent.futures.ThreadPoolExecutor(max_workers=100) as executor:
    futures = [executor.submit(make_request, i) for i in range(1500)]
    for future in concurrent.futures.as_completed(futures):
        pass  # This ensures all requests are completed before finishing
```

## Observations
Table with the number of open cursors for each user before running the python script:
```
USER_NAME OPEN CURSORS
--------- ------------
SYS       372
SYSTEM    4
<null>    0
```

Table with the number of open cursors for each user after running the python script (WebApplication1 project running):
```
USER_NAME OPEN CURSORS
--------- ------------
SYS       473
SYSTEM    357
<null>    192
```

As soon as the WebApplication1 project is stopped, the number of open cursors for each user goes back to the initial state.
When keeping the app running and running the python script again, the number of open cursors **do not** increase anymore. This is wanted behavior, as the cursors are being closed properly or reused. -> No cursor leak.