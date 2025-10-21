using System;

namespace AccountingApp.DTO
{
 public class CustomerDto
 {
 public int Id { get; set; }
 public string Name { get; set; }
 public string Company { get; set; }
 public string ContactPerson { get; set; }
 public string Email { get; set; }
 public string Phone { get; set; }
 public string OrgNumber { get; set; }
 public DateTime CreatedAt { get; set; }
 }
}