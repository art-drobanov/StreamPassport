Imports System.IO
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports StreamPassport

<TestClass>
Public Class StreamPassportTest

    <TestMethod>
    Public Sub StreamPassportTest1()
        Dim rnd As New Random(Now.Ticks Mod Integer.MaxValue)
        For dataByteCount = 1 To 9000
            'Поток-оригинал
            Dim data = New Byte(dataByteCount - 1) {} : rnd.NextBytes(data)
            Dim stream = New MemoryStream() 'Точно так же можно контролировать целостность файлов
            stream.Write(data, 0, data.Length) : stream.Flush()

            Dim streamPassportA = StreamPassport1.Create("StreamA", stream)
            Assert.IsTrue(streamPassportA.IsValid)

            'Проверка на корректность сериализации
            Dim streamPassportStr = streamPassportA.Serialize()
            Dim streamPassport_ = New StreamPassport1()
            streamPassport_.Deserialize(streamPassportStr)
            Assert.IsTrue(streamPassport_.IsValid)
            Assert.IsTrue(streamPassport_.Compare(streamPassportA))
            Assert.IsTrue(streamPassportA.Compare(streamPassport_))

            'Поток-оригинал, дополненный 1 байтом
            stream.WriteByte(255) : stream.Flush()
            Dim streamPassportB = StreamPassport1.Create("StreamB", stream)
            Assert.IsTrue(streamPassportB.IsValid())
            Assert.IsFalse(streamPassportA.Compare(streamPassportB))
            Assert.IsFalse(streamPassportB.Compare(streamPassportA))

            'Поток-оригинал, с 1 измененным байтом
            Dim data2 = data.Clone()
            Dim val = data2(0) : val += 1
            val = If(val > 255, 0, val)
            data2(0) = val

            Dim streamPassportC = StreamPassport1.Create("StreamC", stream)
            Assert.IsTrue(streamPassportC.IsValid())
            Assert.IsFalse(streamPassportA.Compare(streamPassportC))
            Assert.IsFalse(streamPassportC.Compare(streamPassportA))
        Next
    End Sub

    <TestMethod>
    Public Sub StreamPassportTest2()
        Dim rnd As New Random(Now.Ticks Mod Integer.MaxValue)
        For dataByteCount = 1 To 9000
            'Поток-оригинал
            Dim data = New Byte(dataByteCount - 1) {} : rnd.NextBytes(data)
            Dim stream = New MemoryStream() 'Точно так же можно контролировать целостность файлов
            stream.Write(data, 0, data.Length) : stream.Flush()

            Dim streamPassportA = StreamPassport2.Create("StreamA", stream)
            Assert.IsTrue(streamPassportA.IsValid)

            'Проверка на корректность сериализации
            Dim streamPassportStr = streamPassportA.Serialize()
            Dim streamPassport_ = New StreamPassport2()
            streamPassport_.Deserialize(streamPassportStr)
            Assert.IsTrue(streamPassport_.IsValid)
            Assert.IsTrue(streamPassport_.Compare(streamPassportA))
            Assert.IsTrue(streamPassportA.Compare(streamPassport_))

            'Поток-оригинал, дополненный 1 байтом
            stream.WriteByte(255) : stream.Flush()
            Dim streamPassportB = StreamPassport2.Create("StreamB", stream)
            Assert.IsTrue(streamPassportB.IsValid())
            Assert.IsFalse(streamPassportA.Compare(streamPassportB))
            Assert.IsFalse(streamPassportB.Compare(streamPassportA))

            'Поток-оригинал, с 1 измененным байтом
            Dim data2 = data.Clone()
            Dim val = data2(0) : val += 1
            val = If(val > 255, 0, val)
            data2(0) = val

            Dim streamPassportC = StreamPassport2.Create("StreamC", stream)
            Assert.IsTrue(streamPassportC.IsValid())
            Assert.IsFalse(streamPassportA.Compare(streamPassportC))
            Assert.IsFalse(streamPassportC.Compare(streamPassportA))
        Next
    End Sub
End Class
