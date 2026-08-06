export async function getPagedTasks(todoListId, pageNum, pageSize, token) {
    const url = `/api/TodoList/${todoListId}/Tasks/paged?pageNum=${pageNum}&pageSize=${pageSize}`
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch tasks (${response.status})`);
    }

    const data = await response.json();

    return data;
}

export async function getTask(todoListId, taskId, token) {
    const url = `api/TodoList/${todoListId}/Tasks/${taskId}`
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    return await response.json();
}

export async function createTask(todoListId, task, token) {
    const url = `/api/TodoList/${todoListId}/Tasks`
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(task)
    });

    if (!res.ok) {
        throw new Error('Failed to create task');
    }

    return res.json();
}

export async function deleteTask(todoListId, taskId, token) {
    const url = `/api/TodoList/${todoListId}/Tasks/${taskId}`
    const response = await fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to delete task (status ${response.status})`);
    }

    return true;
}

export async function editTask(todoListId, taskId, newTask, token) {
    const url = `/api/TodoList/${todoListId}/Tasks?id=${taskId}`
    const response = await fetch(url, {
        method: 'PUT',
        body: JSON.stringify(newTask),
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        }
    });

    if (!response.ok) {
        throw new Error('Failed to edit task');
    }

    return true;
}

export default {
    getPagedTasks,
    getTask,
    createTask,
    deleteTask,
    editTask
}