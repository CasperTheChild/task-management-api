import PageButton from '../components/PageButton.jsx'
import EditTodoListForm from './EditTodoListForm.jsx'
import { useState, useEffect } from 'react'
import CreateTodoListForm from './CreateTodoListForm.jsx'
import apiService from '../api/TodoListApi.js'
import { useAuth } from '../auth/AuthContext.jsx'

export default function TodoListTable({ setTodoListId }) {
    const { token } = useAuth();

    const [editTodoListId, setEditTodoListId] = useState(null);
    const [editTodoListData, setEditTodoListData] = useState({
        title: "",
        description: "",
        startDate: "",
    }
    );

    function openEditTodoListId(id) {
        const itemToEdit = todoList.items.find(t => t.id === id);

        if (itemToEdit) {
            setEditTodoListId(id);
            setEditTodoListData(itemToEdit);
        }
    }

    function closeEditTodoListForm() {
        setEditTodoListId(null);
    }

        const [todoList, setTodoList] = useState({
        items: [],
        totalCount: 0,
        pageNum: 1,
        pageSize: 4,
        totalPages: 0,
    })

    const [pageSize, setPageSize] = useState(4)
    const [pageNum, setPageNum] = useState(1)

    useEffect(() => {
        apiService.getPagedTodoList(pageNum, pageSize, token)
            .then(data => setTodoList(data));
    }, [pageNum, pageSize])

    async function handleCreateTodoList(newTodoList) {
        try {
            await apiService.createTodoList(newTodoList, token);
            const updated = await apiService.getPagedTodoList(pageNum, pageSize, token);
            setTodoList(updated);
        }
        catch (error) {
            console.error("Failed to create todo list:", error);
        }
    }

    async function handleDeleteTodoList(id) {
        try {
            await apiService.deleteTodoList(id, token);
            const updated = await apiService.getPagedTodoList(pageNum, pageSize, token);
            setTodoList(updated);
        }
        catch (error) {
            console.error("Failed to create todo list:", error);
        }
    }

    async function handleEditTodoList(id, newTodoList) {
        try {
            await apiService.editTodoList(id, newTodoList, token);
            const updated = await apiService.getPagedTodoList(pageNum, pageSize, token);
            setTodoList(updated);
        }
        catch (error) {
            console.error("Failed to create todo list:", error);
        }
    }

    return (
        <div
            className="container-fluid border border-primary col"
        >
            <h1>Todo Lists</h1>
            <table>
                <thead>
                </thead>
                <tbody>
                    {todoList.items.length > 0 &&
                        todoList.items.map(todo => (
                            <tr key={todo.id}>
                                <td>{todo.title}</td>
                                <td>{todo.description}</td>
                                <td>{todo.startDate}</td>
                                <td>{todo.hasOverdueTask}</td>
                                <td><button onClick={() => handleDeleteTodoList(todo.id)}>Delete</button></td>
                                <td><button onClick={() => openEditTodoListId(todo.id)}>Edit</button></td>
                                <td><button onClick={() => setTodoListId(todo.id)}>Tasks</button></td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>

            {todoList.totalPages !== pageNum && (
                <PageButton
                    setValue={setPageNum}
                    val={pageNum + 1}
                />
            )}

            {pageNum != 1 && (
                <PageButton
                    setValue={setPageNum}
                    val={pageNum - 1}
                />
            )}

            {editTodoListId !== null && (
                <div>
                    <EditTodoListForm
                        id={editTodoListId}
                        todo={editTodoListData}
                        onEdit={handleEditTodoList}
                        onClose={closeEditTodoListForm}
                    />
                </div>
            )}


            <CreateTodoListForm
                onCreate={handleCreateTodoList}
            />
        </div>
    )
}