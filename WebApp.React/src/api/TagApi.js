export async function getAllPagedTodoListTags(todoListId, pageNum, pageSize, token) {
    const url = `/api/TodoList/${todoListId}/Tags/paged?pageNum=${pageNum}&pageSize=${pageSize}`
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch tags (${response.status})`);
    }

    return await response.json();
}

export async function getPagedTasksByTagId(todoListId, tagId, pageNum, pageSize, token) {
    const url = `/api/TodoList/${todoListId}/Tags/TagId/${tagId}/paged`;
    const response = await fetch(url, {
        method: 'Get',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch tasks (${response.status})`);
    }

    return await response.json();
}

export async function getPagedTagsByTaskId(todoListId, taskId, pageNum, pageSize, token) {
    const url = `/api/TodoList/${todoListId}/Tags/TaskId/${taskId}/paged`;
    const response = await fetch(url, {
        mathod: 'Get',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to fech tags by taskId: (${response.status})`);
    }

    return await response.json();
}

export async function AssignTagToTask(todoListId, taskId, tagId, token) {
    const url = `api/TodoList/${todoListId}/Tags/Assign`;
    const response = await fetch(url, {
        method: 'Post',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })

    if (!response.ok) {
        throw new Error(`Failed to assign the tag to the task: (${response.status})`)
    }

    return true;
}

export async function RemoveTagFromTask(todoListId, taskId, tagId, token) {
    const url = `api/TodoList/${todoListId}/Tags/Remove`;
    const response = await fetch(url, {
        method: 'Post',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })

    if (!response.ok) {
        throw new Error(`Failed to assign the tag to the task: (${response.status})`)
    }

    return await true;
}

export async function CreateTag(todoListId, name, token) {
    const url = `api/TodoList/${todoListId}/Tags`;
    const response = await fetch(url, {
        method: 'Post',
        body: JSON.stringify(name),
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })

    if (!response.ok) {
        throw new Error(`Failed to assign the tag to the task: (${response.status})`)
    }

    return await true;
}

