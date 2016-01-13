<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Data and Log files on the same physical drive" id="DatabaseDataLogFilesIssue.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Data and Log files on the same physical drive" id="DatabaseDataLogFilesIssue.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Database and Log files on the same physical disk</h1>

				<p>A reader emailed me with a question: if you only have two drive arrays in a server (C
				and D, let's say), and you also have an application on that server, how should you
				configure SQL Server? Best practices say to keep your data and log files on separate
				drive arrays, but is it OK to put them on the same drive and put the application files
				on the other drive?</p>

				<p>The reason the data file (mdf) and log file (ldf) should normally be on separate
				drives is that when you're doing inserts/updates/deletes, they both have to write at the
				exact same time. When the SQL Server engine starts to write data, it essentially:</p>

				<ul>

					<li>Writes to the log file that it's going to change the data</li>

					<li>Writes to the data file</li>

					<li>Writes to the log file to mark that the transaction finished</li>

				</ul>

				<p>That means putting those on the same set of drives is going to make writes much
				slower, because the writes will always be random: the disks will jump from the log file
				location to the data file location and back to the log file location. Random activity is
				slower than sequential activity, so the penalty is even worse.</p>

				<p>So when is it appropriate to put the data and log files on the same drive, and use
				the leftover drive for something else?</p>

				<ul>

					<li><strong>When the database is rarely updated.</strong> If it's mostly read-only,
					you won't incur the heavy penalty on writes.</li>

					<li><strong>When one drive is dramatically faster than the other drive.</strong> Say
					you've got one RAID 10 array with 8 hard drives, and one RAID 5 array with 3
					hard drives. It may make sense to have everything on the fast drive.</li>

					<li><strong>When your application hammers TempDB more than anything else.</strong> I
					had a job interview once where the company said TempDB was more active than their
					data or log file drives by far. Ouch, in that case, it might make sense to
					put TempDB on its own array, and the user database data and log files on the other
					array.</li>

					<li><strong>When the application does heavier disk activity than SQL
					Server.</strong> Notice that I said heavier disk activity: it's not just
					enough to say that the application is used a lot, or does a lot of work it
					has to be disk activity. Some applications just use the disk when they first start
					up, and in that case, they don't merit a separate drive array.</li>

				</ul>

				<p>Regardless of your decision, though, use Perfmon after the system goes live and track
				the drive activity. If one of the two arrays is being overwhelmed with load while the
				other one sits idle, then it's time to rethink the decision. Moving data or log files is
				as easy as detaching the database, copying the physical file (don't move it something
				could go wrong) and reattaching the database. Granted, it involves downtime, but it's
				better than being stuck with your decision for life.</p>

				<p>Update on moving database files: JMKehayias, KBrianKelley and SQLDBA on Twitter all
				chimed in that users would be better off using ALTER DATABASE rather than a
				detach/reattach. At first I was suspicious, not seeing an advantage, but SQLDBA sold me
				when he said they'd moved a log shipped database that way: alter database to change the
				file paths, take it offline, copy the files to their new locations, and bring it back
				online. It kept log shipping intact. That's a win.</p>

				<p><strong>Level of difficulty:</strong> Moderate.</p>

				<p><strong>Level of severity:</strong> Moderate.</p>

				<p><a href="http://www.brentozar.com/archive/2009/02/when-should-you-put-data-and-logs-on-the-same-drive" target="_blank">Database and Log files on the same physical disk</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

