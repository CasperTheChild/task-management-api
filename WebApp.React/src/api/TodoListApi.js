async function getPagedTodoList(pageNum, pageSize, token) {
    const url = `/api/TodoList/paged?pageNum=${pageNum}&pageSize=${pageSize}`;
    const response = await fetch(url, {
        method: 'GET',
        headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}` }},
    );
    const data = await response.json();
    return data;
}

async function createTodoList(todoList, token) {
    const url = `/api/TodoList`
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`, },
        body: JSON.stringify(todoList),
    });

    if (!res.ok) {
        throw new Error('Failed to create todo list');
    }

    return res.json();
}

async function deleteTodoList(id, token) {
    const url = `/api/TodoList/${id}`
    const response = await fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        }
    });

    if (response.status === 204) {
        return true;
    }

    throw new Error("Failed to delete todo list");
}

async function editTodoList(id, newTodoList, token) {
    const url = `/api/TodoList/${id}`
    const res = await fetch(url, {
        method: 'PUT',
        body: JSON.stringify(newTodoList),
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        }
    })

    if (!res.ok) {
        throw new Error('Failed to create todo list');
    }

    return true;
}

export default {
    getPagedTodoList,
    createTodoList,
    deleteTodoList,
    editTodoList,
};