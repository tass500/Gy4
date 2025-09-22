using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TenantService.Models
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Domain is required")]
        [StringLength(100, ErrorMessage = "Domain cannot be longer than 100 characters")]
        public string Domain { get; set; } = string.Empty;
        
        [StringLength(255, ErrorMessage = "Connection string cannot be longer than 255 characters")]
        public string? ConnectionString { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ModifiedAt { get; set; }
    }
}
