@echo 新建数据库[mcsdb]
@echo **危险！！！如果数据库[mcsdb]已存在，此操作将完全删除数据库[mcsdb]后重新创建**
@pause

@echo @osql -U sa -n -i install.sql

@echo 插入数据
@osql -U sa -n -i data.sql

@echo 数据库升级
@osql -U sa -n -i update_1.sql

@pause