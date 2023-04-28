# Cron9

*This repo is in toy stage*

It supports following cron expression
```
millisecond second minute hour day month year weekday weekOfMonth weekOfYear
millisecond: PART ;
second: PART ;
minute: PART ;
hour: PART ;
day: PART ;
month: PART ;
year: PART ;
weekday: PART;
weekOfMonth: PART;
weekOfYear: PART;
PART: RANGE(/Every)? ;
RANGE: * | SINGLE | DOUBLE ;
SINGLE: \d+ ;
DOUBLE: \d+-\d+ ;
EVERY: \d+ ;
```
