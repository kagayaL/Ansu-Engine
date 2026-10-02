using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit;

namespace Tests.RunTime
{
    public class TiledConverterTests
    {
        [Fact]
        public void Convert_ЛогикаПроверяетсяОднимУтверждением()
        {
            // Arrange — готовим вход
            var path = "Data/sample.tmj";

            // Act — делаем то, что тестируем
            var map = TiledConverter.Convert(path);

            // Assert — проверяем результат
            Assert.Equal(30, map.Width);
        }

        [Fact]
        public void Polymorphic_Deserialize_Works()
        {
            string json = """
            {
              "layers": [
                { "type": "tilelayer", "id": 1, "name": "Ground", "width": 30, "height": 20, "data": [1,2,3] }
              ]
            }
            """;
            var map = JsonSerializer.Deserialize<TiledFormat>(json);
            Assert.NotNull(map);
            Assert.Single(map.Layers!);
        }
    }
}
