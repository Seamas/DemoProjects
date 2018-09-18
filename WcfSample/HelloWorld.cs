namespace WcfSample
{
    public class HelloWorld: IHelloWorld
    {
        public string SayHello(string name)
        {
            return "hello: " + name;
        }
    }
}