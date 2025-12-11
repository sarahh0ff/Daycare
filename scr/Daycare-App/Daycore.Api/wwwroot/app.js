// ==========================================================
//  CONFIGURACIÓN BASE
// ==========================================================

const API_BASE = "https://localhost:7235/api";

const $ = (sel) => document.querySelector(sel);
const $$ = (sel) => Array.from(document.querySelectorAll(sel));

function show(el) { el && el.classList.remove("hidden"); }
function hide(el) { el && el.classList.add("hidden"); }

// Mensajes pequeños debajo de cada formulario
function setChildMessage(text) {
    const msg = $("#childFormMessage");
    if (msg) msg.textContent = text || "";
}
function setGuardianMessage(text) {
    const msg = $("#guardianFormMessage");
    if (msg) msg.textContent = text || "";
}
function setActivityMessage(text) {
    const msg = $("#activityFormMessage");
    if (msg) msg.textContent = text || "";
}
function setAttendanceMessage(text) {
    const msg = $("#attendanceFormMessage");
    if (msg) msg.textContent = text || "";
}

// Llamadas genéricas a la API
async function apiRequest(path, { method = "GET", body = null } = {}) {
    const headers = {};
    if (body !== null) headers["Content-Type"] = "application/json";

    const res = await fetch(`${API_BASE}${path}`, {
        method,
        headers,
        body: body ? JSON.stringify(body) : null
    });

    if (!res.ok) {
        const text = await res.text();
        console.error("Error API", res.status, text);
        throw new Error(`Error API ${res.status}`);
    }

    return res.status === 204 ? null : res.json();
}

// ==========================================================
//  HELPERS DE FECHA / HORA SOLO PARA MOSTRAR EN LA TABLA
// ==========================================================

function formatDateOnly(value) {
    if (!value) return "";
    try {
        const str = String(value);
        if (str.includes("T")) return str.split("T")[0];
        const d = new Date(str);
        if (isNaN(d)) return str.slice(0, 10);
        const y = d.getFullYear();
        const m = String(d.getMonth() + 1).padStart(2, "0");
        const day = String(d.getDate()).padStart(2, "0");
        return `${y}-${m}-${day}`;
    } catch {
        return String(value).slice(0, 10);
    }
}

function formatTimeOnly(value) {
    if (!value) return "";
    try {
        const str = String(value);
        if (str.includes("T")) return str.split("T")[1].slice(0, 5);
        const [h = "00", m = "00"] = str.split(":");
        return `${h.padStart(2, "0")}:${m.padStart(2, "0")}`;
    } catch {
        return "";
    }
}

// ==========================================================
//  MÓDULO: NIÑOS
// ==========================================================

async function loadChildren() {
    const tbody = $("#childrenTableBody");
    const countLabel = $("#childrenCount");
    if (!tbody) return;

    tbody.innerHTML = `<tr><td colspan="7" class="table-empty">Cargando niños...</td></tr>`;
    if (countLabel) countLabel.textContent = "…";

    try {
        const data = await apiRequest("/Children");

        if (!Array.isArray(data) || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="table-empty">No hay niños registrados.</td></tr>`;
            if (countLabel) countLabel.textContent = "0";
            populateAttendanceChildSelect([]);
            return;
        }

        tbody.innerHTML = "";
        data.forEach(child => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${child.id}</td>
                <td>${child.firstName}</td>
                <td>${child.lastName}</td>
                <td>${child.email ?? ""}</td>
                <td>${child.phoneNumber ?? ""}</td>
                <td>${child.enrollmentNumber ?? ""}</td>
                <td>
                    <button type="button" class="btn-delete" data-id="${child.id}">
                        Eliminar
                    </button>
                </td>
            `;
            tbody.appendChild(tr);
        });

        if (countLabel) countLabel.textContent = String(data.length);
        populateAttendanceChildSelect(data);
    } catch (err) {
        console.error(err);
        tbody.innerHTML = `
            <tr><td colspan="7" class="table-empty">
                Error al cargar niños: ${err.message}
            </td></tr>`;
        if (countLabel) countLabel.textContent = "!";
    }
}

async function createChild(e) {
    e.preventDefault();
    setChildMessage("");

    const firstName = $("#childFirstName")?.value.trim() ?? "";
    const lastName = $("#childLastName")?.value.trim() ?? "";
    const dateOfBirth = $("#childDateOfBirth")?.value ?? "";
    const email = $("#childEmail")?.value.trim() ?? "";
    const phoneNumber = $("#childPhone")?.value.trim() ?? "";
    const enrollmentNumber = $("#childEnrollmentNumber")?.value.trim() || "CH-AUTO";
    const allergies = $("#childAllergies")?.value.trim() || "";
    const notes = $("#childNotes")?.value.trim() || "";

    if (!firstName || !lastName || !dateOfBirth) {
        setChildMessage("Nombre, apellido y fecha de nacimiento son obligatorios.");
        return;
    }

    const body = {
        firstName,
        lastName,
        dateOfBirth,                  // DateTime en el DTO
        email,
        phoneNumber,
        enrollmentNumber,
        enrollmentDate: new Date().toISOString(),
        allergies,
        notes,
        guardianId: null
    };

    try {
        await apiRequest("/Children", { method: "POST", body });
        setChildMessage("Niño guardado correctamente 💚");
        $("#childForm")?.reset();
        await loadChildren();
    } catch (err) {
        console.error(err);
        setChildMessage("Error al guardar el niño: " + err.message);
    }
}

function initChildDeleteHandler() {
    const tbody = $("#childrenTableBody");
    if (!tbody) return;

    tbody.addEventListener("click", async (e) => {
        const btn = e.target.closest(".btn-delete");
        if (!btn) return;

        const id = btn.dataset.id;
        if (!id) return;

        if (!confirm("¿Seguro que quieres eliminar este niño?")) return;

        try {
            await apiRequest(`/Children/${id}`, { method: "DELETE" });
            setChildMessage("Niño eliminado correctamente 💚");
            await loadChildren();
        } catch (err) {
            console.error(err);
            setChildMessage("Error al eliminar el niño: " + err.message);
        }
    });
}

// ==========================================================
//  MÓDULO: TUTORES
// ==========================================================

async function loadGuardians() {
    const tbody = $("#guardiansTableBody");
    const countLabel = $("#guardiansCount");
    if (!tbody) return;

    tbody.innerHTML = `<tr><td colspan="8" class="table-empty">Cargando tutores...</td></tr>`;
    if (countLabel) countLabel.textContent = "…";

    try {
        const data = await apiRequest("/Guardians");

        if (!Array.isArray(data) || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="8" class="table-empty">No hay tutores registrados.</td></tr>`;
            if (countLabel) countLabel.textContent = "0";
            return;
        }

        tbody.innerHTML = "";
        data.forEach(g => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${g.id}</td>
                <td>${g.firstName}</td>
                <td>${g.lastName}</td>
                <td>${g.email ?? ""}</td>
                <td>${g.phoneNumber ?? ""}</td>
                <td>${g.relationship ?? ""}</td>
                <td>${g.isEmergencyContact ? "Sí" : "No"}</td>
                <td>
                    <button type="button" class="btn-delete-guardian" data-id="${g.id}">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });

        if (countLabel) countLabel.textContent = String(data.length);
    } catch (err) {
        console.error(err);
        tbody.innerHTML = `
            <tr><td colspan="8" class="table-empty">
                Error al cargar tutores: ${err.message}
            </td></tr>`;
        if (countLabel) countLabel.textContent = "!";
    }
}

async function createGuardian(e) {
    e.preventDefault();
    setGuardianMessage("");

    const firstName = $("#guardianFirstName")?.value.trim() ?? "";
    const lastName = $("#guardianLastName")?.value.trim() ?? "";
    const phoneNumber = $("#guardianPhone")?.value.trim() ?? "";
    const email = $("#guardianEmail")?.value.trim() ?? "";
    const relationship = $("#guardianRelationship")?.value.trim() ?? "";
    const isEmergencyContact = $("#guardianEmergency")?.checked ?? false;

    if (!firstName || !lastName) {
        setGuardianMessage("Nombre y apellido del tutor son obligatorios.");
        return;
    }

    const body = {
        firstName,
        lastName,
        phoneNumber,
        email,
        relationship,
        isEmergencyContact
    };

    try {
        await apiRequest("/Guardians", { method: "POST", body });
        setGuardianMessage("Tutor guardado correctamente 💚");
        $("#guardianForm")?.reset();
        await loadGuardians();
    } catch (err) {
        console.error(err);
        setGuardianMessage("Error al guardar el tutor: " + err.message);
    }
}

function initGuardianDeleteHandler() {
    const tbody = $("#guardiansTableBody");
    if (!tbody) return;

    tbody.addEventListener("click", async (e) => {
        const btn = e.target.closest(".btn-delete-guardian");
        if (!btn) return;

        const id = btn.dataset.id;
        if (!id) return;

        if (!confirm("¿Seguro que quieres eliminar este tutor?")) return;

        try {
            await apiRequest(`/Guardians/${id}`, { method: "DELETE" });
            setGuardianMessage("Tutor eliminado correctamente 💚");
            await loadGuardians();
        } catch (err) {
            console.error(err);
            setGuardianMessage("Error al eliminar el tutor: " + err.message);
        }
    });
}

// ==========================================================
//  MÓDULO: ACTIVIDADES
// ==========================================================

async function loadActivities() {
    const tbody = $("#activitiesTableBody");
    if (!tbody) return;

    // Ahora la tabla tiene 7 columnas (Id, Nombre, Fecha, Inicio, Fin, Notas, Acciones)
    tbody.innerHTML = `<tr><td colspan="7" class="table-empty">Cargando actividades...</td></tr>`;

    try {
        const data = await apiRequest("/Activities");

        if (!Array.isArray(data) || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="table-empty">No hay actividades registradas.</td></tr>`;
            return;
        }

        tbody.innerHTML = "";
        data.forEach(a => {
            const name = (a.name ?? a.activityName ?? "(sin nombre)").toString();
            const start = a.startTime ?? a.start ?? null;
            const end = a.endTime ?? a.end ?? null;
            const notes = a.notes ?? a.description ?? "";

            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${a.id}</td>
                <td>${name}</td>
                <td>${formatDateOnly(start)}</td>
                <td>${formatTimeOnly(start)}</td>
                <td>${formatTimeOnly(end)}</td>
                <td>${notes}</td>
                <td>
                    <button type="button" class="btn-delete-activity" data-id="${a.id}">
                        Eliminar
                    </button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error(err);
        tbody.innerHTML = `
            <tr><td colspan="7" class="table-empty">
                Error al cargar actividades: ${err.message}
            </td></tr>`;
    }
}

async function createActivity(e) {
    e.preventDefault();
    setActivityMessage("");

    const nameInput = $("#activityTitle") || $("#activityName");
    const descInput = $("#activityDescription");
    const dateInput = $("#activityDate");
    const startInput = $("#activityStartTime") || $("#activityStart");
    const endInput = $("#activityEndTime") || $("#activityEnd");
    const notesInput = $("#activityNotes");

    if (!nameInput || !dateInput || !startInput || !endInput) {
        setActivityMessage("Faltan campos del formulario.");
        return;
    }

    const name = nameInput.value.trim();
    const description = descInput ? descInput.value.trim() : "";
    const notes = notesInput ? notesInput.value.trim() : "";
    const date = dateInput.value;       // "yyyy-MM-dd"
    const startTime = startInput.value; // "HH:mm"
    const endTime = endInput.value;     // "HH:mm"

    if (!name || !date || !startTime || !endTime) {
        setActivityMessage("Nombre, fecha, inicio y fin son obligatorios.");
        return;
    }

    const startDateTime = `${date}T${startTime}`;
    const endDateTime = `${date}T${endTime}`;

    const body = {
        name,
        description,
        startTime: startDateTime,
        endTime: endDateTime,
        notes
    };

    try {
        await apiRequest("/Activities", { method: "POST", body });
        setActivityMessage("Actividad guardada correctamente 💚");
        $("#activityForm")?.reset();
        await loadActivities();
    } catch (err) {
        console.error(err);
        setActivityMessage("Error al guardar la actividad: " + err.message);
    }
}

function initActivityDeleteHandler() {
    const tbody = $("#activitiesTableBody");
    if (!tbody) return;

    tbody.addEventListener("click", async (e) => {
        const btn = e.target.closest(".btn-delete-activity");
        if (!btn) return;

        const id = btn.dataset.id;
        if (!id) return;

        if (!confirm("¿Seguro que quieres eliminar esta actividad?")) return;

        try {
            await apiRequest(`/Activities/${id}`, { method: "DELETE" });
            setActivityMessage("Actividad eliminada correctamente 💚");
            await loadActivities();
        } catch (err) {
            console.error(err);
            setActivityMessage("Error al eliminar la actividad: " + err.message);
        }
    });
}

// ==========================================================
//  MÓDULO: ASISTENCIAS
// ==========================================================

function populateAttendanceChildSelect(childrenArray) {
    const select = $("#attendanceChildId");
    if (!select) return;

    // Si no recibimos array, lo cargamos desde API
    if (!childrenArray) {
        apiRequest("/Children")
            .then(data => populateAttendanceChildSelect(data))
            .catch(err => console.error("Error al cargar niños para asistencia:", err));
        return;
    }

    select.innerHTML = `<option value="">Selecciona un niño...</option>`;
    childrenArray.forEach(ch => {
        const opt = document.createElement("option");
        opt.value = ch.id;
        opt.textContent = `${ch.firstName} ${ch.lastName}`;
        select.appendChild(opt);
    });
}

async function loadAttendance() {
    const tbody = $("#attendanceTableBody");
    if (!tbody) return;

    tbody.innerHTML = `<tr><td colspan="8" class="table-empty">Cargando asistencias...</td></tr>`;

    try {
        const data = await apiRequest("/Attendances");

        if (!Array.isArray(data) || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="8" class="table-empty">No hay asistencias registradas.</td></tr>`;
            return;
        }

        tbody.innerHTML = "";
        data.forEach(a => {
            const childName =
                a.childName ||
                (a.child
                    ? `${a.child.firstName ?? ""} ${a.child.lastName ?? ""}`.trim()
                    : `Niño #${a.childId ?? ""}`);

            const dateRaw = a.date ?? a.attendanceDate ?? null;
            const status = a.status ?? "";
            const checkInRaw = a.checkInTime ?? a.checkIn ?? null;
            const checkOutRaw = a.checkOutTime ?? a.checkOut ?? null;
            const notes = a.notes ?? "";

            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${a.id}</td>
                <td>${childName}</td>
                <td>${formatDateOnly(dateRaw)}</td>
                <td>${status}</td>
                <td>${formatTimeOnly(checkInRaw)}</td>
                <td>${formatTimeOnly(checkOutRaw)}</td>
                <td>${notes}</td>
                <td>
                    <button type="button" class="btn-delete-attendance" data-id="${a.id}">
                        Eliminar
                    </button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error(err);
        tbody.innerHTML = `
            <tr><td colspan="8" class="table-empty">
                Error al cargar asistencias: ${err.message}
            </td></tr>`;
    }
}

async function createAttendance(e) {
    e.preventDefault();
    setAttendanceMessage("");

    const childId = $("#attendanceChildId")?.value ?? "";
    const date = $("#attendanceDate")?.value ?? "";
    const status = $("#attendanceStatus")?.value ?? "";
    const checkIn = $("#attendanceCheckIn")?.value ?? "";
    const checkOut = $("#attendanceCheckOut")?.value ?? "";
    const notes = $("#attendanceNotes")?.value.trim() ?? "";

    if (!childId || !date || !status) {
        setAttendanceMessage("Niño, fecha y estado son obligatorios.");
        return;
    }

    const checkInTime = checkIn ? `${date}T${checkIn}` : null;
    const checkOutTime = checkOut ? `${date}T${checkOut}` : null;

    const body = {
        childId: Number(childId),
        attendanceDate: date,    // nombre alineado al modelo del backend
        status,
        checkIn,                 // si tu modelo usa solo hora, cambia a checkInTime
        checkOut,                // idem
        checkInTime,             // por si tu backend tiene este campo
        checkOutTime,
        notes
    };

    try {
        await apiRequest("/Attendances", { method: "POST", body });
        setAttendanceMessage("Asistencia guardada correctamente 💚");
        $("#attendanceForm")?.reset();
        await loadAttendance();
    } catch (err) {
        console.error(err);
        setAttendanceMessage("Error al guardar la asistencia: " + err.message);
    }
}

function initAttendanceDeleteHandler() {
    const tbody = $("#attendanceTableBody");
    if (!tbody) return;

    tbody.addEventListener("click", async (e) => {
        const btn = e.target.closest(".btn-delete-attendance");
        if (!btn) return;

        const id = btn.dataset.id;
        if (!id) return;

        if (!confirm("¿Seguro que quieres eliminar este registro de asistencia?")) return;

        try {
            await apiRequest(`/Attendances/${id}`, { method: "DELETE" });
            setAttendanceMessage("Asistencia eliminada correctamente 💚");
            await loadAttendance();
        } catch (err) {
            console.error(err);
            setAttendanceMessage("Error al eliminar la asistencia: " + err.message);
        }
    });
}

// ==========================================================
//  NAVEGACIÓN LATERAL
// ==========================================================

function initNavigation() {
    const btnChildren = $("#btnViewChildren");
    const btnGuardians = $("#btnViewGuardians");
    const btnActivities = $("#btnViewActivities");
    const btnAttendance = $("#btnViewAttendance");

    const childrenView = $("#childrenView");
    const guardiansView = $("#guardiansView");
    const activitiesView = $("#activitiesView");
    const attendanceView = $("#attendanceView");

    function activate(view) {
        [childrenView, guardiansView, activitiesView, attendanceView]
            .filter(Boolean)
            .forEach(hide);

        if (view) show(view);
        $$(".sidebar-btn").forEach(b => b.classList.remove("sidebar-btn--active"));
    }

    if (btnChildren && childrenView) {
        btnChildren.addEventListener("click", () => {
            activate(childrenView);
            btnChildren.classList.add("sidebar-btn--active");
        });
    }

    if (btnGuardians && guardiansView) {
        btnGuardians.addEventListener("click", () => {
            activate(guardiansView);
            btnGuardians.classList.add("sidebar-btn--active");
            loadGuardians();
        });
    }

    if (btnActivities && activitiesView) {
        btnActivities.addEventListener("click", () => {
            activate(activitiesView);
            btnActivities.classList.add("sidebar-btn--active");
            loadActivities();
        });
    }

    if (btnAttendance && attendanceView) {
        btnAttendance.addEventListener("click", () => {
            activate(attendanceView);
            btnAttendance.classList.add("sidebar-btn--active");
            loadAttendance();
            populateAttendanceChildSelect();
        });
    }
}

// ==========================================================
//  INIT GLOBAL
// ==========================================================

window.addEventListener("DOMContentLoaded", () => {
    initNavigation();
    initChildDeleteHandler();
    initGuardianDeleteHandler();
    initActivityDeleteHandler();
    initAttendanceDeleteHandler();

    // Eventos niños
    $("#childForm")?.addEventListener("submit", createChild);
    $("#btnReloadChildren")?.addEventListener("click", loadChildren);

    // Eventos tutores
    $("#guardianForm")?.addEventListener("submit", createGuardian);
    $("#btnReloadGuardians")?.addEventListener("click", loadGuardians);

    // Eventos actividades
    $("#activityForm")?.addEventListener("submit", createActivity);
    $("#btnReloadActivities")?.addEventListener("click", loadActivities);

    // Eventos asistencias
    $("#attendanceForm")?.addEventListener("submit", createAttendance);
    $("#btnReloadAttendance")?.addEventListener("click", loadAttendance);

    // Carga inicial
    loadChildren();          
    populateAttendanceChildSelect();
});
