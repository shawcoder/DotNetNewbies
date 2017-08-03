using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStoreDal.SpyStore.DAL.EF.Repository.Base
{
	using Models.Entities.Base;

	public interface IRepositoryBase<T> where T: EntityBase
  {
		int Count { get; }
		bool HasChanges { get; }
	  T Find(int? aId);
	  T GetFirst();
	  IEnumerable<T> GetAll();
	  IEnumerable<T> GetRange(int aSkip, int aTake);
	  int Add(T aEntity, bool aPersist = true);
	  int AddRange(IEnumerable<T> aEntities, bool aPersist = true);
	  int Update(T aEntity, bool aPersist = true);
	  int UpdateRange(IEnumerable<T> aEntities, bool aPersist = true);
	  int Delete(T aEntity, bool aPersist = true);
	  int DeleteRange(IEnumerable<T> aEntities, bool aPersist = true);
	  int Delete(int aId, byte[] aTimeStamp, bool aPersist = true);
	  int SaveChanges();
  }

}
