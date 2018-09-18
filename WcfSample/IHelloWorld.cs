using System;
using System.ServiceModel;

namespace WcfSample
{
    [ServiceContract]
    public interface IHelloWorld
    {
        [OperationContract]
        string SayHello(string name);
    }
}