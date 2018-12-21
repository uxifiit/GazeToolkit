using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Statistics.Serialization
{
    public class SamplesCountCsvConverter : CsvConverter<SamplesCount>
    {
        public override bool CanRead => false;

        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            throw new NotSupportedException();
        }

        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(SamplesCount.Filter)));
            writer.WriteField(naming.Get(nameof(SamplesCount.InputSamples)));
            writer.WriteField(naming.Get(nameof(SamplesCount.OutputSamples)));
            writer.WriteField(naming.Get(nameof(SamplesCount.Runtime)));
        }

        protected override void WriteCsv(SamplesCount data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Filter);
            writer.WriteField(data.InputSamples);
            writer.WriteField(data.OutputSamples);
            writer.WriteField(data.Runtime);
        }
    }
}
