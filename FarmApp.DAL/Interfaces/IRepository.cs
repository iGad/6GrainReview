using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.Interfaces
{
    //1. Все операции синхронные
    //2. Func<TEntity, bool> надо заменить на Expression<Func<TEntity, bool>>, т.к. в первом случае фильтрация выполнится после получения данных из БД, а во втором случае условие оно преобразуется в SQL выражение where
    //3. Из сигнатуры Update(TEntity item) непонятно, что выполняет метод. Либо сделать Update(TEntity oldItem, TEntity newItem), либо просто получать сущность через Get и сохранять изменения в SaveChanges
    //4. IEnumerable<TEntity> Get(); убрать. Из названия непонятно, что он делает (логичнее было бы назвать GetAll); этот метод является частным случаем Get(x=>true); в реальном проекте крайне редко требуется получать все записи в таблице, только лишний раз провоцирует программиста сделать ошибку.
    //5. Create переименовать в Add, ведь он не создает сущность, а добавляет её. Вместо void можно возвращать TEntity, т.к. после добавления у сущности появится идентификатор и, теоретически, в какой-нибудь ORM добавится копия item
    //6. ограничение where TEntity : class стоит заменить на TEntity: Entity, в Entity определить свойство int Id {get;set;} в качестве ключа, т.к. TEntity FindById(int id); работает только с такими сущностями
    //Я бы убрал IUnitOfWork, а в IRepository добавил бы SaveChangesAsync. 
    /// <summary>
    /// Обобщённый репозиторий - набор операций над сущностью
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
	{
		void Create(TEntity item);

		TEntity FindById(int id);

		IEnumerable<TEntity> Get();

		IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

		void Remove(TEntity item);

		void Update(TEntity item);
	}
}
