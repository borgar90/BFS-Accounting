using System;
using System.Collections.Generic;
using AccountingApp.DataAccess;
using AccountingApp.DTO;

namespace AccountingApp.Model
{
 public class ProductModel
 {
 private ProductRepository repo = new ProductRepository();
 public IEnumerable<ProductDto> GetAll()
 {
 var list = new List<ProductDto>();
 foreach (var p in repo.GetAll())
 {
 list.Add(new ProductDto { Id = p.Id, ProductNumber = p.ProductNumber, Name = p.Name, Price = p.Price, IsCustom = p.IsCustom });
 }
 return list;
 }
 public ProductDto GetById(int id)
 {
 var p = repo.GetById(id);
 if (p == null) return null;
 return new ProductDto { Id = p.Id, ProductNumber = p.ProductNumber, Name = p.Name, Price = p.Price, IsCustom = p.IsCustom };
 }
 public int Add(ProductDto p) => repo.Add(new ProductDto { ProductNumber = p.ProductNumber, Name = p.Name, Price = p.Price, IsCustom = p.IsCustom });
 public void Update(ProductDto p) => repo.Update(new ProductDto { Id = p.Id, ProductNumber = p.ProductNumber, Name = p.Name, Price = p.Price, IsCustom = p.IsCustom });
 public void Delete(int id) => repo.Delete(id);
 }
}