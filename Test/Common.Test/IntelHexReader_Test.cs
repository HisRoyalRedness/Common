using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HisRoyalRedness.com.IntelHex;

namespace HisRoyalRedness.com
{
    [TestClass]
    public class IntelHexReader_Tests
    {
        [TestMethod]
        public void IntelHexReader_EmptyStreamIsValid()
        {
            var reader = new IntelHexReader("");

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0);
            reader.EndOfStream.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_NewlineBeforeStartCodeIsValid()
        {
            var reader = new IntelHexReader("\r\n\n\r");

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0);
            reader.EndOfStream.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_CharsBeforeStartCodeAreIgnored()
        {
            var reader = new IntelHexReader("123abcqwe()$");

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0);
            reader.EndOfStream.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_InvalidCharThrows()
        {
            var reader = new IntelHexReader("\n:x");

            var data = new byte[40];
            var func = new Func<int>(() => reader.Read(data, 0, data.Length));

            var exAssert = func.Should().Throw<FileFormatException>();
            var ex = exAssert.Subject.First();

            ex.LineNumber.Should().Be(2);
            ex.ColumnNumber.Should().Be(2);
        }

        [TestMethod]
        public void IntelHexReader_ReadByteCountAddressAndRecordType()
        {
            var record = new Record();
            var reader = new IntelHexReader(":12345678", record);

            var data = new byte[40];
            new Func<int>(() => reader.Read(data, 0, data.Length)).Should().Throw<FileFormatException>();

            record.ByteCount.Should().Be(0x12);
            reader.CurrentAddress.Should().Be(0x3456);
            record.RecordType.Should().Be(0x78);
        }

        [TestMethod]
        public void IntelHexReader_ReadLessDataThanLineLength()
        {
            var record = new Record();
            var reader = new IntelHexReader(":20C0200031313131313100000300000000000000D90400002D2D2D2D2D424547494E204351", record);

            var data = new byte[10];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(10);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x31, 0x31, 0x31, 0x31, 0x31,
                0x31, 0x00, 0x00, 0x03, 0x00
            });
            record.LineCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_ReadSameDataThanLineLength()
        {
            var record = new Record();
            var reader = new IntelHexReader(":20C4E000697936614668306A7A49446438513D0D0A2D2D2D2D2D454E44204345525449469E", record);

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0x20);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x69, 0x79, 0x36, 0x61, 0x46, 0x68, 0x30, 0x6A, 
                0x7A, 0x49, 0x44, 0x64, 0x38, 0x51, 0x3D, 0x0D, 
                0x0A, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x45, 0x4E, 
                0x44, 0x20, 0x43, 0x45, 0x52, 0x54, 0x49, 0x46
            });
            record.LineCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_ReadMoreDataThanLineLength()
        {
            var record = new Record();
            var reader = new IntelHexReader(
                ":20C0200031313131313100000300000000000000D90400002D2D2D2D2D424547494E204351\r\n" +
                ":20C04000455254494649434154452D2D2D2D2D0D0A4D49494457444343417636674177496F", record);

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0x28);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x00, 0x00,
                0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xD9, 0x04, 0x00, 0x00, 0x2D, 0x2D, 0x2D, 0x2D,
                0x2D, 0x42, 0x45, 0x47, 0x49, 0x4E, 0x20, 0x43,
                0x45, 0x52, 0x54, 0x49, 0x46, 0x49, 0x43, 0x41
            });
            record.LineCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_MultipleSmallerReadsSpanningLines()
        {
            var record = new Record();
            var reader = new IntelHexReader(
                ":20C0200031313131313100000300000000000000D90400002D2D2D2D2D424547494E204351\r\n" +
                ":20C04000455254494649434154452D2D2D2D2D0D0A4D49494457444343417636674177496F", record);
            var data = new byte[13];

            // Read 1
            var bytesRead = reader.Read(data, 0, data.Length);
            bytesRead.Should().Be(13);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x31, 0x31, 0x31, 0x31, 0x31,
                0x31, 0x00, 0x00, 0x03, 0x00,
                0x00, 0x00, 0x00
            });

            // Read 2
            bytesRead = reader.Read(data, 0, data.Length);
            bytesRead.Should().Be(13);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x00, 0x00, 0x00, 0xD9, 0x04, 
                0x00, 0x00, 0x2D, 0x2D, 0x2D,
                0x2D, 0x2D, 0x42
            });

            // Read 3
            bytesRead = reader.Read(data, 0, data.Length);
            bytesRead.Should().Be(13);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x45, 0x47, 0x49, 0x4E, 0x20,
                0x43, 0x45, 0x52, 0x54, 0x49, 
                0x46, 0x49, 0x43
            });

            // Read 4
            bytesRead = reader.Read(data, 0, data.Length);
            bytesRead.Should().Be(13);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x41, 0x54, 0x45, 0x2D, 0x2D, 
                0x2D, 0x2D, 0x2D, 0x0D, 0x0A, 
                0x4D, 0x49, 0x49
            });

            // Read 4
            bytesRead = reader.Read(data, 0, data.Length);
            bytesRead.Should().Be(12);
            data.Select(b => (int)b).Take(bytesRead).Should().Equal(new[] {
                0x44, 0x57, 0x44, 0x43, 0x43, 
                0x41, 0x76, 0x36, 0x67, 0x41, 
                0x77, 0x49
            });
        }

        [TestMethod]
        public void IntelHexReader_ParseEndOfFileRecord()
        {
            var record = new Record();
            var reader = new IntelHexReader(":00000001FF\r\n", record);

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0);
            record.LineCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void IntelHexReader_NoFurtherRecordsAfterEndOfFile()
        {
            var record = new Record();
            var reader = new IntelHexReader(":00000001FF\r\n   :", record);

            var data = new byte[40];
            var func = new Func<int>(() => reader.Read(data, 0, data.Length));

            var exAssert = func.Should().Throw<FileFormatException>();
            var ex = exAssert.Subject.First();

            ex.LineNumber.Should().Be(2);
            ex.ColumnNumber.Should().Be(4);
        }

        [TestMethod]
        public void IntelHexReader_ParseExtendedSegmentAddress()
        {
            var record = new Record();
            var reader = new IntelHexReader(":020000020102F9", record);

            var data = new byte[40];
            var bytesRead = reader.Read(data, 0, data.Length);

            bytesRead.Should().Be(0);
            reader.CurrentAddress.Should().Be(0x01020);
        }

        [TestMethod]
        public void IntelHexReader_CheckExtendedSegmentAddress()
        {
            var record = new Record();
            var reader = new IntelHexReader(
                ":020000020102F9\r\n" +
                ":020010001234A8\r\n" +
                ":02003000567800", record);

            reader.Read().Should().Be(0x12);
            reader.CurrentAddress.Should().Be(0x01031);

            reader.Read().Should().Be(0x34);
            reader.CurrentAddress.Should().Be(0x01032);

            reader.Read().Should().Be(0x56);
            reader.CurrentAddress.Should().Be(0x01051);

            reader.Read().Should().Be(0x78);
            reader.CurrentAddress.Should().Be(0x01052);
        }

        // :020000040800F2
    }
}
