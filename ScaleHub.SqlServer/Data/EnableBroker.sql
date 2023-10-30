DECLARE @IsBrokerEnabled INT;
SELECT @IsBrokerEnabled = is_broker_enabled
FROM sys.databases
WHERE name = '{0}';
IF @IsBrokerEnabled = 0
BEGIN
    ALTER DATABASE scalehub SET ENABLE_BROKER;
END;