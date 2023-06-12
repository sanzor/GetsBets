-- Table: public.extragere

-- DROP TABLE IF EXISTS public.extragere;

CREATE TABLE IF NOT EXISTS public.extragere
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( CYCLE INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    data_extragere date NOT NULL,
    ora_extragere time without time zone,
    numere text COLLATE pg_catalog."default" NOT NULL,
    bonus text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT extragere_pkey PRIMARY KEY (id),
    CONSTRAINT unique_extragere_data UNIQUE (data_extragere, ora_extragere, numere, bonus)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.extragere
    OWNER to postgres;