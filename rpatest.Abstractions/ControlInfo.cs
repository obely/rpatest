namespace rpatest.Abstractions
{
    public record ControlInfo
    {
        public string Parent { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
    }
}
