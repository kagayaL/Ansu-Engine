using Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.RunTime
{
    public class TiledConverterTests
    {
        [Fact]
        public void Convert_ЛогикаПроверяетсяОднимУтверждением()
        {
            // Arrange — готовим вход
            string path = "C:/Users/Denis/Desktop/FillerName/src/Tests/RunTime/Data/sample.tmj";

            // Act — делаем то, что тестируем
            var map = TiledConverter.Convert(path);

            // Assert — проверяем результат
            Assert.Equal(30, map.Width);
        }

        [Fact]
        public void MapDataTest()
        {
            string tmjPath = "C:/Users/Denis/Desktop/FillerName/src/Tests/RunTime/Data/sample.tmj";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var map = TiledConverter.Convert(tmjPath);
            var json = JsonSerializer.Serialize<MapData>(map, options);
            Console.WriteLine(json);
        }
    }
}
