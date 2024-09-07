namespace SpiceCraft.Server.DTO.ItemImage
{
    public class ItemImageDTO
    {
        public int ImageId { get; set; }

        public int ItemId { get; set; }

        public string ImageCode { get; set; } = null!;

        public string? ImageName { get; set; }

        public int? ImageIndex { get; set; }

        public bool? IsMain { get; set; }

    }
}
