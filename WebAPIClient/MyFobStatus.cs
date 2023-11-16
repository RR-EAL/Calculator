internal record MyFobStatus
{
        public string PrimaryKey { get; set; }
        public int CurrentPosition { get; set; }
        public string HomeSystemPrimaryKey { get; set; }
        public int HomePosition { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string CurrentUserForeignKey { get; set; }
}
