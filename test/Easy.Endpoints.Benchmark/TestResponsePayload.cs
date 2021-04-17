namespace Easy.Endpoints.Benchmark
{
    public class TestResponsePayload
    {
        public string PropertyOne { get; set; }
        public int PropertyTwo { get; set; }

        public static TestResponsePayload Default => new TestResponsePayload { PropertyOne = "TEaafjkoajgojao", PropertyTwo = 2352 };
    }
}
