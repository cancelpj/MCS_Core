@echo �½����ݿ�[mcsdb]
@echo **Σ�գ�����������ݿ�[mcsdb]�Ѵ��ڣ��˲�������ȫɾ�����ݿ�[mcsdb]�����´���**
@pause

@echo @osql -U sa -n -i install.sql

@echo ��������
@osql -U sa -n -i data.sql

@echo ���ݿ�����
@osql -U sa -n -i update_1.sql

@pause