<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Тестирование нескольких отчётов с разделителями" id="4" columns="33;33;34" rows="50;50" splitter="yes">

		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" column="1" row="1" colspan="1" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>MSSQLResults</PathToItems>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Имя"                   HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Суммарное"             HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Данные"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Данные (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Журнал"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Журнал (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" column="2" row="1" colspan="1" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>MSSQLResults</PathToItems>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Имя"                   HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Суммарное"             HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Данные"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Данные (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Журнал"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Журнал (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" column="3" row="1" colspan="1" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>MSSQLResults</PathToItems>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Имя"                   HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Суммарное"             HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Данные"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Данные (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Журнал"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Журнал (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" column="1" row="2" colspan="3" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>MSSQLResults</PathToItems>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Имя"                   HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Суммарное"             HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Данные"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Данные (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Журнал"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Журнал (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
