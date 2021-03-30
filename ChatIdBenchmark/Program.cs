using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkEquals>();
BenchmarkRunner.Run<BenchmarkNewInstance>();

[MemoryDiagnoser]
public class BenchmarkNewInstance
{
    private const int N = 100;

    readonly ChatIdRef[] chatIdRef1 = new ChatIdRef[N];
    readonly ChatIdRef[] chatIdRef2 = new ChatIdRef[N];
    readonly ChatIdVal[] chatIdVal1 = new ChatIdVal[N];
    readonly ChatIdVal[] chatIdVal2 = new ChatIdVal[N];
    readonly ChatIdRecord[] chatIdRecord1 = new ChatIdRecord[N];
    readonly ChatIdRecord[] chatIdRecord2 = new ChatIdRecord[N];

    [Benchmark(Baseline = true)]
    public bool ReferenceTypeFromLong()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdRef1[i] = new ChatIdRef(value);
        }
        return i == N;
    }

    [Benchmark]
    public bool ReferenceTypeFromString()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdRef2[i] = new ChatIdRef("@abcde");
        }
        return i == N;
    }

    [Benchmark]
    public bool ValueTypeFromLong()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdVal1[i] = new ChatIdVal(value);
        }
        return i == N;
    }

    [Benchmark]
    public bool ValueTypeFromString()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdVal2[i] = new ChatIdVal("@abcde");
        }
        return i == N;
    }

    [Benchmark]
    public bool RecordTypeFromLong()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdRecord1[i] = ChatIdRecord.New(value);
        }
        return i == N;
    }

    [Benchmark]
    public bool RecordTypeFromString()
    {
        var rnd = new Random();
        int i;
        int value;
        for (i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdRecord2[i] = ChatIdRecord.New("@abcde");
        }
        return i == N;
    }

    [GlobalSetup]
    public void Init()
    {
    }
}

[MemoryDiagnoser]
public class BenchmarkEquals
{
    private const int N = 100;

    readonly ChatIdRef[] chatIdRef1 = new ChatIdRef[N];
    readonly ChatIdRef[] chatIdRef2 = new ChatIdRef[N];
    readonly ChatIdVal[] chatIdVal1 = new ChatIdVal[N];
    readonly ChatIdVal[] chatIdVal2 = new ChatIdVal[N];
    readonly ChatIdRecord[] chatIdRecord11 = new ChatIdRecord[N];
    readonly ChatIdRecord[] chatIdRecord12 = new ChatIdRecord[N];
    readonly ChatIdRecord[] chatIdRecord21 = new ChatIdRecord[N];
    readonly ChatIdRecord[] chatIdRecord22 = new ChatIdRecord[N];

    [Benchmark(Baseline = true)]
    public bool ReferenceType()
    {
        int i;
        for (i = 0; i < N; i++)
        {
            _ = chatIdRef1[i] == chatIdRef2[i];
            chatIdRef1[i].Equals(chatIdRef2[i]);
        }
        return i == N;
    }

    [Benchmark]
    public bool ValueType()
    {
        int i;
        for (i = 0; i < N; i++)
        {
            _ = chatIdVal1[i] == (chatIdVal2[i]);
            chatIdVal1[i].Equals(chatIdVal2[i]);
        }
        return i == N;
    }

    [Benchmark]
    public bool RecordTypeUsername()
    {
        int i;
        for (i = 0; i < N; i++)
        {
            _ = (string)chatIdRecord11[i] == chatIdRecord12[i];
            ((string)chatIdRecord11[i]).Equals(chatIdRecord12[i]);
        }
        return i == N;
    }

    [Benchmark]
    public bool RecordTypeIdentifier()
    {
        int i;
        for (i = 0; i < N; i++)
        {
            _ = (long)chatIdRecord21[i] == chatIdRecord22[i];
            ((long)chatIdRecord21[i]).Equals(chatIdRecord22[i]);
        }
        return i == N;
    }

    [GlobalSetup]
    public void Init()
    {
        var rnd = new Random();
        int value;
        for (var i = 0; i < N; i++)
        {
            value = rnd.Next();
            chatIdRef1[i] = new ChatIdRef(value);
            chatIdRef2[i] = new ChatIdRef(value.ToString());
            chatIdVal1[i] = new ChatIdVal(value);
            chatIdVal2[i] = new ChatIdVal(value.ToString());
            chatIdRecord11[i] = ChatIdRecord.New("@abcde");
            chatIdRecord12[i] = ChatIdRecord.New("@abcde");
            chatIdRecord21[i] = ChatIdRecord.New(value);
            chatIdRecord22[i] = ChatIdRecord.New(value);
        }
    }
}
