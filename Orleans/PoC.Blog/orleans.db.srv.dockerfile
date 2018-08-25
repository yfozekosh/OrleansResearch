FROM postgres:latest

ENV POSTGRES_DB PoC_Blog_ClusteringDb
COPY clustering_000_init.sql docker-entrypoint-initdb.d
RUN cat docker-entrypoint-initdb.d/clustering_000_init.sql