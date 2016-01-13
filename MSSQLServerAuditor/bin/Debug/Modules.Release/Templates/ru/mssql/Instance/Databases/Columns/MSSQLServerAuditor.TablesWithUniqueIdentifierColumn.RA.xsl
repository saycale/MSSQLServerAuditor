<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с ключами на основе NewID" id="TablesWithUniqueIdentifierColumn.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с ключами на основе NewID" id="TablesWithUniqueIdentifierColumn.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Совет: не кластеризуйте тип данных UniqueIdentifierUniqueIdentifier при использовании NewId</h1>

				<p>SQL Server хранит данные в порядке кластерного индекса, поэтому в таблице может быть только один кластерный индекс.
				Если Вы используете UniqueIdentifier в качестве первой колонки кластерного индекса, каждый раз при вставлении ряда в таблицу
				он будет помещен в ее центр. SQL сервер хранит данные на страницах размером 8Kилобайт. Если страница заполнена,
				SQL сервер выполнит разрыв страницы, таким образом создавая новую 8 Kилобайтовую странцу и половина данных с предыдущей
				страницы будет перемещена на новую. Разрыв каждой страницы происходит относительно быстро, но иногда может занять некоторое время.
				В рабочей среде с большим количеством транзакций разрывы страниц могут происходит довольно часто, замедляя этим работу программы.</p>

				<p>Когда Вы используете колонку Identity (Идентичность) для кластеризованного индекса, следующее вставленное значение почти всегда будет
				больше предыдущего. Это означает, что новые ряды всегда будут добавляться в конец таблицы, что избавит Вас от ненужных
				разрывов страниц для фрагментации таблицы.</p>

				<p>В SQL Server 2005 представлена новая функция NewSequentialId().Она может быть использована лишь по умолчанию для колонки типа
				UniqueIdentifier. Преимущество функции NewSequentialId в том, что она всегда генерирует значение больше,
				чем уже имеющееся в таблице. В результате чего, новый ряд вставляется в конец таблицы и не вызывает разрывов страницы.</p>

				<p><strong>Как это исправить:</strong> Существует несколько способ избавиться от данной проблемы.
				Лучший способ – использование функции NewSequentialId() вместо NewId. Либо (если Вы используете SQL 2000), следует установить значение индекса фактора
				заполнения меньше, чем 100%. Фактор заполнения определяет, насколько заполнены страницы при повторном создании индекса. Если фактор заполнения – 100%,
				в индексе не остается места для размещения новых рядов. Если необходимо использовать UniqueIdentifier, индекс должен быть кластеризован.
				При этом нельзя использовать функцию NewSequentialId. Следует изменить Фактор Заполнения, чтобы уменьшить количество разрывов страниц.
				В этом случае также важно периодически перестраивать индекс.</p>

				<p><strong>Уровень сложности:</strong> Простой</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/best-practice-don-t-not-cluster-on-uniqu" target="_blank">Best Practice: Do not cluster on UniqueIdentifier when you use NewId</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
