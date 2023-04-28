# Cron9

*This repo is in toy stage*

It supports following cron expression
millisecond second minute hour day month year weekday weekOfMonth weekOfYear
Each part should be format 
PART: RANGE(/Every)? ;
RANGE: * | SINGLE | DOUBLE ;
SINGLE: \d+ ;
DOUBLE: \d+-\d+
EVERY: \d+
