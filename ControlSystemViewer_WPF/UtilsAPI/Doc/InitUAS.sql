update UASUSR set USER_GRP='ADMIN' where USER_ID='ADMIN'
update UASUSR set USER_GRP='ENG' where USER_ID='ENG'
delete UASUSRGRP where USER_GRP in ('ADMIN','ENG')
insert into UASUSRGRP(USER_GRP) values('ADMIN')
insert into UASUSRGRP(USER_GRP) values('ENG')
delete UASFNC
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_CLOSE_MASTER_PC','Close master pc')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_CLOSE_SYSTEM','Close system')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_ENGINEER_FUN','Engineer Fun')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_HIGHT_LEVEL_ACTION','Hight level ation (Load/Unload/Load Unload...)')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_LOGIN','Login Fun')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_MAINTENANCE_FUN','Maintenance Fun')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_OPERATION_FUN','Connection Fun')
insert into UASFNC(FUNC_CODE,FUNC_NAME) values('FUNC_USER_MANAGEMENT','User Management')
delete UASUFNC where USER_GRP in ('ADMIN','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_CLOSE_MASTER_PC','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_CLOSE_SYSTEM','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_ENGINEER_FUN','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_HIGHT_LEVEL_ACTION','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_LOGIN','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_MAINTENANCE_FUN','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_OPERATION_FUN','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_USER_MANAGEMENT','ADMIN')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_CLOSE_MASTER_PC','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_CLOSE_SYSTEM','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_ENGINEER_FUN','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_HIGHT_LEVEL_ACTION','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_LOGIN','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_MAINTENANCE_FUN','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_OPERATION_FUN','ENG')
insert into UASUFNC(FUNC_CODE,USER_GRP) values('FUNC_USER_MANAGEMENT','ENG')