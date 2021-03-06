﻿using HLShop.Common;
using HLShop.Data.Infrastructure;
using HLShop.Data.Repositories;
using HLShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace HLShop.Service
{
    public interface IProductService
    {
        Product Add(Product product);

        void Update(Product product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAllByPaging(int page, string sort, int pageSize, out int totalRow);

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetLastest(int top);

        IEnumerable<Product> GetHotProduct(int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, string sort, int pageSize, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, string sort, int pageSize, out int totalRow);

        IEnumerable<string> GetListProductByName(string keyword);

        IEnumerable<Product> GetListRelatedProduct(int id, int top);

        Product GetById(int id);

        void Save();

        IEnumerable<Tag> GetListTagByProductId(int id);

        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);

        Tag GetTag(string tagId);

        void UpdateQuantity(Product product, int quantity);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository,
            ITagRepository _tagRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = _tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product product)
        {
            var productToAdd = _productRepository.Add(product);
            _unitOfWork.Commit();

            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }

            return productToAdd;
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);

            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    // truoc khi update phai xoa all ProductTag dang co
                    _productTagRepository.DeleteMulti(x => x.ProductID == product.ID);

                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAllByPaging(int page, string sort, int pageSize, out int totalRow)
        {
            var query = _productRepository.GetAll();

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;

                case "priceLowToHight":
                    query = query.OrderBy(x => x.Price);
                    break;

                case "priceHightToLow":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productRepository.GetMulti(x =>
                    x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }
            else
            {
                return _productRepository.GetAll();
            }
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _productRepository.GetMulti(x => x.Status == true && x.HotFlag == true).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, string sort, int pageSize, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status == true && x.CategoryID == categoryId);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;

                case "priceLowToHight":
                    query = query.OrderBy(x => x.Price);
                    break;

                case "priceHightToLow":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> Search(string keyword, int page, string sort, int pageSize, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status == true && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;

                case "priceLowToHight":
                    query = query.OrderBy(x => x.Price);
                    break;

                case "priceHightToLow":
                    query = query.OrderByDescending(x => x.Price);
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string keyword)
        {
            return _productRepository.GetMulti(x => x.Status == true && x.Name.Contains(keyword)).Select(y => y.Name);
        }

        public IEnumerable<Product> GetListRelatedProduct(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository
                .GetMulti(x => x.Status == true && x.ID != id && x.CategoryID == product.CategoryID)
                .OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x
                => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public void IncreaseView(int id)
        {
            var product = _productRepository.GetSingleById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount++;
            else
                product.ViewCount = 1;
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var model = _productRepository.GetListProductByTag(tagId, page, pageSize, out totalRow);
            return model;
        }

        public Tag GetTag(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public void UpdateQuantity(Product product, int quantity)
        {
            if (product.Quantity > quantity)
            {
                product.Quantity = product.Quantity - quantity;
            }
            _productRepository.Update(product);
            _unitOfWork.Commit();
        }
    }
}