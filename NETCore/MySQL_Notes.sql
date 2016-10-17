-- Set Current UTC Timestamp value

UPDATE AppLog
SET LogTimeUtc = UTC_TIMESTAMP()
WHERE AppLogId = 1;

-- Grant a primary field Auto_incremental

ALTER TABLE {Table} MODIFY COLUMN {Column} {INT(11)} AUTO_INCREMENT

