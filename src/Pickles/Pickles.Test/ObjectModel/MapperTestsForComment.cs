using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    [TestFixture]
    public class MapperTestsForComment
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToComment_NullLocation_ReturnsNull()
        {
            var mapper = this.factory.CreateMapper();
            var result = mapper.MapToComment(null);
            Check.That(result).IsNull();
        }

        [Test]
        public void MapToComment_RegularComment_ReturnsComment()
        {
            var mapper = this.factory.CreateMapper();
            var comment = this.factory.CreateComment("# A comment", 1, 2);
            var result = mapper.MapToComment(comment);
            Check.That(result).IsNotNull();
            Check.That(result.Text).IsEqualTo("# A comment");
            Check.That(result.Location.Line).IsEqualTo(1);
            Check.That(result.Location.Column).IsEqualTo(2);
            Check.That(result.Type).IsEqualTo(CommentType.Normal);
        }
    }
}
