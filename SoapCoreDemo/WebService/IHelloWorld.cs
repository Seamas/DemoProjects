using System.ServiceModel;

namespace SoapCoreDemo.WebService
{
    [ServiceContract]
    public interface IHelloWorld
    {
        [OperationContract]
        string SayHello(string name);

        [OperationContract]
        byte[] Download(string name);
    }
}