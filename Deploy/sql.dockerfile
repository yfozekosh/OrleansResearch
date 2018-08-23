FROM microsoft/mssql-server-linux:2017-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=OrleDBP@asswor1d

COPY *.sql .

RUN ((/opt/mssql/bin/sqlservr --accept-eula & ) | /bin/grep -q "Service Broker manager has started") && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'OrleDBP@asswor1d' -i 000_create_db.sql && pkill sqlservr 

EXPOSE 1433
CMD ["/opt/mssql/bin/sqlservr"]