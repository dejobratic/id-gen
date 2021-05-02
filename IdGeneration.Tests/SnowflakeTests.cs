using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdGeneration.Tests
{
    [TestClass]
    [TestCategory("Unit")]
    public class SnowflakeTests
    {
        [TestMethod]
        public void Able_to_generate_id()
        {
            var id = Snowflake.NewId();

            id.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Multiple_generated_ids_should_not_be_equal()
        {
            var idCount = 100000;

            var ids = new ConcurrentBag<long>();
            Parallel.For(0, idCount, _ =>
            {
                ids.Add(Snowflake.NewId());
            });

            ids.Should().HaveCount(idCount);
            ids.Should().BeEquivalentTo(ids.Distinct());
        }

        [TestMethod]
        public void Newer_ids_should_be_greater_than_older_ones()
        {
            var idCount = 100000;

            var ids = new List<long>();
            for (int i = 0; i < idCount; i++)
            {
                ids.Add(Snowflake.NewId());
            }

            ids.Should().BeInAscendingOrder();
            ids.Should().BeEquivalentTo(ids.Distinct());
        }
    }
}
