CREATE OR REPLACE FUNCTION check_rows_count()
RETURNS TRIGGER AS $$
BEGIN
    IF (SELECT COUNT(*) FROM current_weekday) >= 1 THEN
        RAISE EXCEPTION 'Number of records in the table must be less than 1';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_record_count BEFORE
INSERT
    ON current_weekday FOR EACH ROW
EXECUTE FUNCTION check_rows_count ();