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


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref SamplesCount result)
        {
            throw new NotSupportedException();
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(SamplesCount.Filter)));
            writer.WriteField(naming.Get(nameof(SamplesCount.InputSamples)));
            writer.WriteField(naming.Get(nameof(SamplesCount.OutputSamples)));
            writer.WriteField(naming.Get(nameof(SamplesCount.Runtime)));
        }


        protected override void Write(SamplesCount data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Filter);
            writer.WriteField(data.InputSamples);
            writer.WriteField(data.OutputSamples);
            writer.WriteField(data.Runtime);
        }
    }
}
