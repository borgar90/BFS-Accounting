using System.Collections.Generic;
using AccountingApp.Model;
using AccountingApp.DTO;

namespace AccountingApp.Controller
{
 public class ProductController
 {
 private ProductModel model = new ProductModel();
 public IEnumerable<ProductDto> GetAll() => model.GetAll();
 public ProductDto GetById(int id) => model.GetById(id);
 public int Add(ProductDto p) => model.Add(p);
 public void Update(ProductDto p) => model.Update(p);
 public void Delete(int id) => model.Delete(id);
 }
}