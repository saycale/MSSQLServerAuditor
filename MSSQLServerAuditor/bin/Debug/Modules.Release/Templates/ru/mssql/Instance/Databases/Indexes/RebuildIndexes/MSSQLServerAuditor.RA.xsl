<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Индексы для выполнения операции Rebuild" id="RebuildIndexes.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Индексы для выполнения операции Rebuild" id="RebuildIndexes.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Поиск фрагментации индекса и ее исправление</h1>

				<p>При частом обновлении, вставках и удалениях индекс становится фрагментированным.
				Далее я приведу пример создания таблицы, полного ее фрагментирования, последующей
				реорганизации и перестройки индекса.</p>

				<p><strong>Как это исправить:</strong> Есть два способа избавиться от фрагментации –
				заново организовать индекс и перестроить его. Реорганизация индекса выполняется в
				обычном режиме работы сервера, перестройка - вне ее (в специальном режиме регламентного
				обслуживания) за исключением случая, если Вы укажете опцию ONLINE = ON. Эта функция
				поддерживается лишь в Enterprise версиях SQL Server.</p>

				<p>Между REBUILD ONLINE = ON и REBUILD ONLINE = OFF есть два отличия</p>

				<p><strong>ON:</strong> Долгосрочные блокировки таблицы не применяются во время операций
				индекса. В течение основной фазы операции индекса на исходной таблице удерживается
				только блокировка Intent Share (IS). Это позволяет продолжать выполнение запросов или
				обновлений для базовой таблицы и индексов. В начале операции на исходном объекте
				недолгое время удерживается блокировка Shared (S). В конце операции при создании
				некластеризованного индекса на исходном объекте некоторое время действует блокировка
				Shared (S). Блокировка SCH-M (Schema Modification) применяется при создании кластерного
				индекса или его «выброса» в сеть, а также перестройке как кластеризованного, так и не
				кластеризованного индексов. Фукция ONLINE не может быть включена (ON) при создании
				индекса на местной временной таблице. </p>

				<p><strong>OFF:</strong> Блокировки таблицы применяются на время выполнения операции
				индекса. К внесетевым операциям создания, перестройки или удаления из памяти
				кластерного, пространственного или XML индекса, также перестройки и удаления из памяти
				некластерного индекса применяется блокировка таблицы Schema modification (Sch-M). Она
				блокирует доступ всех пользователей к базовой таблице на время проведения операции. Для
				внесетевой операции по созданию некластериного индекса применяется блокировка Shared
				(S). Она блокирует обновления базовой таблицы, но позволяет выполнять операции чтения,
				например SELECT.</p>

				<h1 class="firstHeading">Индексы, для которых нужно провести операцию "Rebuild"</h1>

				<p>В отчёте представлены индексы, для которых рекомендуется провести операцию "Rebuild".</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms189858.aspx" target="_blank">Reorganize and Rebuild Indexes</a></p>

				<p><a href="http://wiki.lessthandot.com/index.php/Finding_Fragmentation_Of_An_Index_And_Fixing_It" target="_blank">Поиск фрагментации индекса и ее исправление</a></p>

				<p><a href="http://blog.sqlauthority.com/2010/01/12/sql-server-fragmentation-detect-fragmentation-and-eliminate-fragmentation/" target="_blank">SQL SERVER – Fragmentation – Detect Fragmentation and Eliminate Fragmentation</a></p>

				<p><a href="http://www.oszone.net/7326/SQL_Server" target="_blank">SQL Server: Лучшие советы по эффективному обслуживанию баз данных</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
