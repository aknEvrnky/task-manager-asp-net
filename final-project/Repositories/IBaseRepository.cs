namespace final_project.Repositories;

public interface IBaseRepository<T>
{
    List<T> All();
    T? Find(int id);
    T Create(T model);
    void Update(T model);
    void Delete(int id);
}