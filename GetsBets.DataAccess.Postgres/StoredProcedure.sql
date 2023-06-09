﻿--CREATE OR REPLACE FUNCTION insert_extraction(p_date date, p_numbers text, p_time time)
--RETURNS VOID AS $$
--BEGIN
--    -- Check if the item already exists
--    IF NOT EXISTS (SELECT 1 FROM extraction WHERE date_column = p_date AND numbers_column = p_numbers AND time_column = p_time) THEN
--        -- Insert the item into the table
--        INSERT INTO extraction (date_column, numbers_column, time_column) VALUES (p_date, p_numbers, p_time);
--    END IF;
--END;
--$$ LANGUAGE plpgsql;