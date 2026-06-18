# solicitorSearchApi
Api for the Solicitor Finder app

The UI that receives data from this API is https://github.com/4emonas/SolicitorFinderUI

This API requires the following database table to be created in postgres:

```
-- Table: public.solicitor

-- DROP TABLE IF EXISTS public.solicitor;

CREATE TABLE IF NOT EXISTS public.solicitor
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    name character varying COLLATE pg_catalog."default" NOT NULL,
    address character varying COLLATE pg_catalog."default",
    "phoneNumber" character varying COLLATE pg_catalog."default",
    notes text COLLATE pg_catalog."default",
    city character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT solicitor_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.solicitor
    OWNER to postgres;
-- Index: solicitor_city_idx

-- DROP INDEX IF EXISTS public.solicitor_city_idx;

CREATE INDEX IF NOT EXISTS solicitor_city_idx
    ON public.solicitor USING btree
    (city COLLATE pg_catalog."default" ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;
-- Index: solicitor_name_city_idx

-- DROP INDEX IF EXISTS public.solicitor_name_city_idx;

CREATE INDEX IF NOT EXISTS solicitor_name_city_idx
    ON public.solicitor USING btree
    (name COLLATE pg_catalog."default" ASC NULLS LAST, city COLLATE pg_catalog."default" ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;
-- Index: solicitor_name_idx

-- DROP INDEX IF EXISTS public.solicitor_name_idx;

CREATE INDEX IF NOT EXISTS solicitor_name_idx
    ON public.solicitor USING btree
    (name COLLATE pg_catalog."default" ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;

```
