FROM postgres:latest

ENV POSTGRES_DB PoC_Blog_BloggingDb
COPY blogging_000_init.sql /docker-entrypoint-initdb.d