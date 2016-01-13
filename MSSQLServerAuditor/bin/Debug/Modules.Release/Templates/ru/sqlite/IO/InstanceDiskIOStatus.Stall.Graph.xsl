<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Задержки ввода/вывода дисков (чтение / запись) (ms)" id="InstanceDiskIOStatus.Stall.Graph.ru" columns="100" rows="50;50" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Задержки ввода/вывода дисков по чтению (ms)" id="InstanceDiskIOStatus.Stall.Reads.Graph.ru" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<FirstWeekDay>
					Monday
				</FirstWeekDay>

				<AxisXConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<AxisXLabelFilter>
						All
					</AxisXLabelFilter>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MinorGrid>
				</AxisXConfiguration>

				<AxisYConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>
					<Format>
						#,###
					</Format>
					<Name>
						N
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDiskIOStatus' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<DateTag>EventTime</DateTag>
					<NameTag>Disk</NameTag>
					<ValueTag>io_stall_read_ms</ValueTag>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Byte
					</Unit>
				</ItemsConfiguration>

				<LegendConfiguration>
					<Enabled>
						true
					</Enabled>
					<Docking>
						Top
					</Docking>
					<Alignment>
						Center
					</Alignment>
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					NamedLine
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Disks I/O read stalls (ms)" id="InstanceDiskIOStatus.Stall.Writes.Graph.ru" column="1" row="2" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<FirstWeekDay>
					Monday
				</FirstWeekDay>

				<AxisXConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<AxisXLabelFilter>
						All
					</AxisXLabelFilter>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MinorGrid>
				</AxisXConfiguration>

				<AxisYConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>
					<Format>
						#,###
					</Format>
					<Name>
						N
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDiskIOStatus' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<DateTag>EventTime</DateTag>
					<NameTag>Disk</NameTag>
					<ValueTag>io_stall_write_ms</ValueTag>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Byte
					</Unit>
				</ItemsConfiguration>

				<LegendConfiguration>
					<Enabled>
						true
					</Enabled>
					<Docking>
						Top
					</Docking>
					<Alignment>
						Center
					</Alignment>
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					NamedLine
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
