docker build -t yfozekosh/mssql-orledb:latest -f sql.dockerfile . 
(docker stop orleSql1 || true) && (docker container rm orleSql1 || true)
docker run -d --name orleSql1 -p 1435:1433 yfozekosh/mssql-orledb:latest