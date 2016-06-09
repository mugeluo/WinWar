
--新闻表
alter table NEWS_MAIN add IS_ISSUE char(1) default '0'
Go
update NEWS_MAIN set IS_ISSUE='1'


alter table NEWS_MAIN add NEWS_TYPE int 


update NEWS_MAIN set NEWS_TYPE=16
