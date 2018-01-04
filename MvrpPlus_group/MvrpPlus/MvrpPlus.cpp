// MvrpPlus.cpp : Defines the entry point for the console application.
/* Copyright (C) 2014 Xuesong Zhou - All Rights Reserved*/
/* Copyright (C) 2015 Xuesong Zhou and junhua chen - All Rights Reserved*/
/*Contact Info: xzhou99@gmail.com, cjh@bjtu.edu.cn*/
/* this code is developed as part of research paper
Finding Optimal Solutions for Vehicle Routing Problem with Pickup and Delivery Services with Time Windows: A Dynamic Programming Approach Based on State-space-time Network Representations
Monirehalsadat Mahmoudi, Xuesong Zhou
http://arxiv.org/abs/1507.02731
*/
/*
MvrpPlus Commercial License for OEMs, ISVs and VARs
The source code authors (Xuesong Zhou and Monirehalsadat Mahmoudi ) provides its MvrpPlus Libraries under a dual license model designed 
to meet the development and distribution needs of both commercial distributors (such as OEMs, ISVs and VARs) and open source projects.

For OEMs, ISVs, VARs and Other Distributors of Commercial Applications:
OEMs (Original Equipment Manufacturers), ISVs (Independent Software Vendors), VARs (Value Added Resellers) and other distributors that
combine and distribute commercially licensed software with MvrpPlus software and do not wish to distribute the source code for the commercially licensed software 
under version 2 of the GNU General Public License (the "GPL") must enter into a commercial license agreement with the source code authors.

For Open Source Projects and Other Developers of Open Source Applications:
For developers of Free Open Source Software ("FOSS") applications under the GPL that want to combine and distribute those FOSS applications with 
MvrpPlus software, our MvrpPlus open source software licensed under the GPL is the best option.

For developers and distributors of open source software under a FOSS license other than the GPL, we make the GPL-licensed MvrpPlus Libraries available
under a FOSS Exception that enables use of the those MvrpPlus Libraries under certain conditions without causing the entire derivative work to be subject to the GPL.
*/

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// The one and only application object
#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <omp.h>
#include <algorithm>
#include <time.h>
#include "MvrpPlus.h"
#include "CSVParser.h"

CWinApp theApp;
using namespace std;

FILE* g_pFileDebugLog = NULL;
FILE* g_pFileOutputForMainProgram = NULL;
FILE* g_pFile_All_Routes = NULL;
FILE* g_pFileOutputLog = NULL;
FILE* g_pFile_Map_GS_Str = NULL;
int g_number_of_threads = 6;

bool g_debug_out = true;

std::map<int, int> g_internal_node_no_map;
std::map<int, int> g_external_node_id_map;

std::map<string, int> g_internal_agent_no_map;
std::map<int, string> g_external_passenger_id_map;
std::map<int, string> g_external_vehicle_id_map;

std::map<string, int>g_fromNode_ToNode_map_linkID;

float g_passenger_base_profit[_MAX_NUMBER_OF_PASSENGERS] = { 7 };
float g_passenger_request_travel_time_vector[_MAX_NUMBER_OF_PASSENGERS] = { 999 };
int g_accessibility_matrix[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_demand[_MAX_NUMBER_OF_PASSENGERS] = { 1 };


int g_number_of_passengers = 0;

int g_total_group_number = 0;
int g_max_v_in_group = 0; ////get value from configuration
int g_max_p_in_group = 0;////get value from configuration
int g_max_task_accomplish = 6;  ////get value from configuration
int g_min_task_accomplish = 2; /// get value from configuration
//int g_max_vehicle_capacity = 2; ////get value from configuration
int g_memory_for_states = 2100;////get value from configuration

int g_v_p_shortest_path_maxtrix[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS];

int g_group_p[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_PASSENGERS] = { 0 };
int g_group_v[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_VEHICLES] = { 0 };

int g_vehicle_earliest_pickup_passenger[_MAX_NUMBER_OF_VEHICLES] = {0};
int g_vehicle_earliest_pickup_time[_MAX_NUMBER_OF_VEHICLES] = { 0 };
int g_vehicle_latest_deliver_passenger[_MAX_NUMBER_OF_VEHICLES] = { 0 };
int g_vehicle_latest_deliver_time[_MAX_NUMBER_OF_VEHICLES] = { 0 };


int g_map_groupNode_to_globalNode[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_NODES] = { 0 };
int g_map_globalNode_to_groupNode[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_NODES] = { 0 };
int g_group_node[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_NODES] = { 0 };
int g_group_link[_MAX_NUMBER_OF_GROUP][_MAX_NUMBER_OF_LINKS] = { 0 };


float g_parameter_c[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_STATES] = { _MAX_LABEL_COST };
int g_parameter_a[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_STATES] = { 0 };
int g_decision_x[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_STATES] = { 0 };


int g_vehicle_serving_passenger_matrix[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS] = { 0 };

float g_arc_travel_time[_MAX_NUMBER_OF_LINKS][_MAX_NUMBER_OF_TIME_INTERVALS] = { 0 };
int g_vertex_visit_count_for_lower_bound[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_TIME_INTERVALS] = { 0 };  // used for lower bound

float** g_vehicle_origin_based_node_travel_time = NULL;
float** g_vehicle_destination_based_node_travel_time = NULL;


// int -->string
void int2str(const int &int_temp, string &string_temp)
{
	stringstream stream;
	stream << int_temp;
	string_temp = stream.str();
}
// string --> int
int str2int(const string &string_temp)
{
	int a;
	stringstream stream(string_temp);
	stream >> a;
	return a;
}

string int2str(int input_int)
{
	stringstream stream;
	stream << input_int;
	return stream.str();
}

string connect_2_int(int from_int, int to_int)
{
	string str_from;
	string str_to;
	int2str(from_int, str_from); int2str(to_int, str_to);
	stringstream str_connect;
	str_connect << str_from; str_connect << "-"; str_connect << str_to;
	return str_connect.str();
}

#pragma region <input data>


int g_outbound_node_size[_MAX_NUMBER_OF_NODES] = { 0 };
int g_node_passenger_id[_MAX_NUMBER_OF_NODES] = { 0 };
int g_node_passenger_pickup_flag[_MAX_NUMBER_OF_NODES] = { 0 };

int g_outbound_node_id[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_OUTBOUND_NODES];
int g_outbound_link_no[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_OUTBOUND_NODES];
int g_activity_node_flag[_MAX_NUMBER_OF_NODES] = { 0 };
int g_activity_node_ending_time[_MAX_NUMBER_OF_NODES] = { 99999 };
int g_activity_node_starting_time[_MAX_NUMBER_OF_NODES] = { 99999 };

int g_inbound_node_size[_MAX_NUMBER_OF_NODES];
int g_inbound_node_id[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_INBOUND_NODES];
int g_inbound_link_no[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_INBOUND_NODES];


int g_link_free_flow_travel_time[_MAX_NUMBER_OF_LINKS];
float g_link_free_flow_travel_time_float_value[_MAX_NUMBER_OF_LINKS];

float g_link_link_length[_MAX_NUMBER_OF_LINKS];
int g_link_number_of_lanes[_MAX_NUMBER_OF_LINKS];
int g_link_mode_code[_MAX_NUMBER_OF_LINKS];
float g_link_capacity_per_time_interval[_MAX_NUMBER_OF_LINKS];
float g_link_jam_density[_MAX_NUMBER_OF_LINKS];
int g_link_service_code[_MAX_NUMBER_OF_LINKS] = { 0 };


float g_link_speed[_MAX_NUMBER_OF_LINKS];
int g_link_from_node_id[_MAX_NUMBER_OF_LINKS];
int g_link_to_node_id[_MAX_NUMBER_OF_LINKS];


float g_VOIVTT_per_hour[_MAX_NUMBER_OF_VEHICLES];
float g_VOWT_per_hour[_MAX_NUMBER_OF_VEHICLES];



int g_vehicle_path_number_of_nodes[_MAX_NUMBER_OF_VEHICLES] = { 0 };

float g_vehicle_path_cost[_MAX_NUMBER_OF_VEHICLES] = { 0 };  // for vehcile routings

int g_vehicle_origin_node[_MAX_NUMBER_OF_VEHICLES];  // for vehcile routings
int g_vehicle_departure_time_beginning[_MAX_NUMBER_OF_VEHICLES] = { 0 };
int g_vehicle_departure_time_ending[_MAX_NUMBER_OF_VEHICLES];
int g_vehicle_destination_node[_MAX_NUMBER_OF_VEHICLES];
int g_vehicle_arrival_time_beginning[_MAX_NUMBER_OF_VEHICLES] = { 120 };
int g_vehicle_arrival_time_ending[_MAX_NUMBER_OF_VEHICLES];

int g_passenger_origin_node[_MAX_NUMBER_OF_PASSENGERS];  // traveling passengers
int g_passenger_departure_time_beginning[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_departure_time_ending[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_destination_node[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_dummy_destination_node[_MAX_NUMBER_OF_PASSENGERS] = { -1 };


int g_passenger_arrival_time_beginning[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_arrival_time_ending[_MAX_NUMBER_OF_PASSENGERS];
float g_lamda_vehicle_passenger_assignment_multiplier[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS] = { 0 };


float g_passenger_request_cancelation_cost[_MAX_NUMBER_OF_PASSENGERS] = { 0 };


int g_vehicle_capacity[_MAX_NUMBER_OF_VEHICLES] = { 1 };

int g_passenger_assigned_vehicle_id[_MAX_NUMBER_OF_PASSENGERS] = { 0 };
int g_passenger_path_node_sequence[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_TIME_INTERVALS];
int g_passenger_path_link_sequence[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_TIME_INTERVALS];
int g_passenger_path_time_sequence[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_TIME_INTERVALS];
int g_passenger_path_number_of_nodes[_MAX_NUMBER_OF_PASSENGERS] = { 0 };

int g_path_node_sequence[_MAX_NUMBER_OF_TIME_INTERVALS];
int g_path_link_sequence[_MAX_NUMBER_OF_TIME_INTERVALS];
int g_path_time_sequence[_MAX_NUMBER_OF_TIME_INTERVALS];
int g_path_number_of_nodes;
float g_path_travel_time[_MAX_NUMBER_OF_VEHICLES] = { 0 };


int g_number_of_links = 0;
int g_number_of_nodes = 0;
int g_number_of_physical_nodes = 0;
int g_maximum_number_name_of_nodes = 0;

int g_number_of_time_intervals = 10;



int g_number_of_vehicles = 0;
int g_number_of_physical_vehicles = 0;


int g_number_of_toll_records = 0;

//int g_number_of_LR_iterations = 1;
//int g_minimum_subgradient_step_size = 1;

int g_shortest_path_debugging_flag = 0;
float g_waiting_time_ratio = 0.005;
float g_dummy_vehicle_cost_per_hour = 120;




int g_add_new_node(int passenger_id, int beginning_time = -1, int end_time = -1)
{
	int new_node_internal_id = g_number_of_nodes;
	int new_node_external_number = g_maximum_number_name_of_nodes + 1;

	g_outbound_node_size[new_node_internal_id] = 0;
	g_inbound_node_size[new_node_internal_id] = 0;
	g_node_passenger_id[new_node_internal_id] = passenger_id;
	g_activity_node_flag[new_node_internal_id] = 1;
	g_activity_node_starting_time[new_node_internal_id] = beginning_time;
	g_activity_node_ending_time[new_node_internal_id] = end_time;

	g_internal_node_no_map[new_node_external_number] = new_node_internal_id;
	g_external_node_id_map[new_node_internal_id] = new_node_external_number;

	g_number_of_nodes++;
	g_maximum_number_name_of_nodes++;


	return new_node_internal_id;
}

int g_add_new_link(int from_node_id, int to_node_id, int passenger_id = 0, int travel_time = 1, double link_length = 1, int number_of_lanes = 1, int mode_code = 0,
	int capacity_per_time_interval = 1, double speed = 60)
{
	int new_link_id = g_number_of_links;

	g_outbound_node_id[from_node_id][g_outbound_node_size[from_node_id]] = to_node_id;
	g_outbound_link_no[from_node_id][g_outbound_node_size[from_node_id]] = new_link_id;

	g_outbound_node_size[from_node_id]++;

	g_inbound_node_id[to_node_id][g_inbound_node_size[to_node_id]] = from_node_id;
	g_inbound_link_no[to_node_id][g_inbound_node_size[to_node_id]] = new_link_id;
	g_inbound_node_size[to_node_id]++;


	g_link_from_node_id[new_link_id] = from_node_id;
	g_link_to_node_id[new_link_id] = to_node_id;

	g_fromNode_ToNode_map_linkID[connect_2_int(from_node_id, to_node_id)] = new_link_id;

	g_link_free_flow_travel_time[new_link_id] = max(1, travel_time);

	g_link_link_length[g_number_of_links] = link_length;
	g_link_number_of_lanes[g_number_of_links] = number_of_lanes;
	g_link_mode_code[g_number_of_links] = mode_code;
	g_link_capacity_per_time_interval[g_number_of_links] = capacity_per_time_interval;
	g_link_speed[g_number_of_links] = speed;
	g_link_service_code[g_number_of_links] = passenger_id;

	g_number_of_links++;
	return g_number_of_links;

}



int g_get_link_no_based_on_from_node_to_node(int from_node, int to_node)
{
	if (from_node >= _MAX_NUMBER_OF_NODES)
		return -1;

	if (from_node == to_node)
		return -1;

	//scan outbound links from a upstream node 
	for (int i = 0; i < g_outbound_node_size[from_node]; i++)
	{
		if (g_outbound_node_id[from_node][i] == to_node)
			return g_outbound_link_no[from_node][i];
	}

	return -1;

}


void g_ReadInputData()
{

	// initialization
	for (int i = 0; i < _MAX_NUMBER_OF_NODES; i++)
	{
		g_outbound_node_size[i] = 0;
		g_inbound_node_size[i] = 0;

	}

	g_number_of_nodes = 0; // initialize  the counter to 0
	g_number_of_links = 0; // initialize  the counter to 0
	g_number_of_time_intervals = 0;

	int interval_node_no = 0;
	// step 1: read node file 
	CCSVParser parser;
	if (parser.OpenCSVFile("input_node.csv", true))
	{
		std::map<int, int> node_id_map;

		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			string name;

			int node_type;
			int node_id;
			double X;
			double Y;
			if (parser.GetValueByFieldName("node_id", node_id) == false)
				continue;


			if (node_id <= 0 || g_number_of_nodes >= _MAX_NUMBER_OF_NODES)
			{
				cout << "node_id " << node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			g_internal_node_no_map[node_id] = interval_node_no;

			g_external_node_id_map[interval_node_no] = node_id;
			interval_node_no++;

			g_maximum_number_name_of_nodes = max(g_maximum_number_name_of_nodes, node_id);

			parser.GetValueByFieldName("node_type", node_type);
			parser.GetValueByFieldName("x", X);
			parser.GetValueByFieldName("y", Y);

			g_number_of_nodes++;
			if (g_number_of_nodes % 1000 == 0)
				cout << "reading " << g_number_of_nodes << " physical nodes.. " << endl;

		}

		cout << "number of physical nodes = " << g_number_of_nodes << endl;

		g_number_of_physical_nodes = g_number_of_nodes;

		fprintf(g_pFileDebugLog, "number of physical nodes =,%d\n", g_number_of_nodes);
		fprintf(g_pFileOutputLog, "number of physical nodes =,%d\n", g_number_of_nodes);

		parser.CloseCSVFile();

	}

	// step 2: read link file 

	if (parser.OpenCSVFile("input_link.csv", true))
	{
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{
			int from_node_id = 0;
			int to_node_id = 0;
			if (parser.GetValueByFieldName("from_node_id", from_node_id) == false)
				continue;
			if (parser.GetValueByFieldName("to_node_id", to_node_id) == false)
				continue;

			if (from_node_id <= 0)
			{
				cout << "from_node_id " << from_node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			if (to_node_id <= 0)
			{
				cout << "to_node_id " << to_node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			if (g_internal_node_no_map.find(from_node_id) == g_internal_node_no_map.end())
			{
				//cout << " warning: from_node_id " << from_node_id << " has not been defined in node input file" << endl;
				continue;
			}

			if (g_internal_node_no_map.find(to_node_id) == g_internal_node_no_map.end())
			{
				// cout << "warning  " << to_node_id << " has not been defined in node input file" << endl;
				continue;
			}

			if (g_internal_node_no_map[from_node_id] >= _MAX_NUMBER_OF_NODES)
			{
				cout << "from_node_id " << from_node_id << " is out of range" << endl;
				g_ProgramStop();
			}
			if (g_internal_node_no_map[to_node_id] >= _MAX_NUMBER_OF_NODES)
			{
				cout << "to_node_id " << to_node_id << " is out of range" << endl;
				g_ProgramStop();
			}
			// add the to node id into the outbound (adjacent) node list

			int direction = 1;
			parser.GetValueByFieldName("direction", direction);

			if (direction <= -2 || direction >= 2)
			{
				cout << "direction " << direction << " is out of range" << endl;
				g_ProgramStop();
			}

			for (int link_direction = -1; link_direction <= 1; link_direction += 2)  // called twice; -1 direction , 1 direction 
			{
				if (direction == -1 && link_direction == 1)
					continue; // skip

				if (direction == 1 && link_direction == -1)
					continue; // skip

				// then if  direction == 0 or 2 then create the corresponding link

				int directional_from_node_id = g_internal_node_no_map[from_node_id];
				int directional_to_node_id = g_internal_node_no_map[to_node_id];


				if (link_direction == -1) // reverse direction;
				{
					directional_from_node_id = g_internal_node_no_map[to_node_id];
					directional_to_node_id = g_internal_node_no_map[from_node_id];
				}

				g_outbound_node_id[directional_from_node_id][g_outbound_node_size[directional_from_node_id]] = directional_to_node_id;
				g_outbound_link_no[directional_from_node_id][g_outbound_node_size[directional_from_node_id]] = g_number_of_links;

				g_outbound_node_size[directional_from_node_id]++;
				g_inbound_node_id[directional_to_node_id][g_inbound_node_size[directional_to_node_id]] = directional_from_node_id;
				g_inbound_link_no[directional_to_node_id][g_inbound_node_size[directional_to_node_id]] = g_number_of_links;
				g_inbound_node_size[directional_to_node_id]++;

				float link_length = 1;
				int number_of_lanes = 1;
				int mode_code = 0;
				float capacity_per_time_interval = 1;
				float travel_time = 1.0;
				float speed = 1;
				float jam_density = 200;


				parser.GetValueByFieldName("length", link_length);
				parser.GetValueByFieldName("number_of_lanes", number_of_lanes);
				parser.GetValueByFieldName("mode_code", mode_code);
				parser.GetValueByFieldName("lane_capacity_in_vhc_per_hour", capacity_per_time_interval);

				parser.GetValueByFieldName("speed_limit", speed);
				if (speed >= 70)
					speed = 70;

				if (speed <= 25)
					speed = 25;


				travel_time = max(1, link_length * 60 / max(1, speed));
				parser.GetValueByFieldName("jam_density", jam_density);


				if (travel_time > 1000)
				{
					cout << "travel time of link " << from_node_id << " -> " << to_node_id << " > 1000";
					g_ProgramStop();
				}

				g_link_from_node_id[g_number_of_links] = directional_from_node_id;
				g_link_to_node_id[g_number_of_links] = directional_to_node_id;
				
				g_fromNode_ToNode_map_linkID[connect_2_int(directional_from_node_id, directional_to_node_id)] = g_number_of_links;

				g_link_free_flow_travel_time[g_number_of_links] = max(1, travel_time + 0.5);   // at least 1 min, round to nearest integers
				g_link_free_flow_travel_time_float_value[g_number_of_links] = travel_time;

				g_link_link_length[g_number_of_links] = link_length;
				g_link_number_of_lanes[g_number_of_links] = number_of_lanes;
				g_link_mode_code[g_number_of_links] = mode_code;
				g_link_capacity_per_time_interval[g_number_of_links] = capacity_per_time_interval;
				g_link_speed[g_number_of_links] = speed;

				// increase the link counter by 1
				g_number_of_links++;

				if (g_number_of_links % 1000 == 0)
					cout << "reading " << g_number_of_links << "physical links.. " << endl;

			}

		}

		cout << "number of physical links = " << g_number_of_links << endl;

		fprintf(g_pFileDebugLog, "number of physical links =,%d\n", g_number_of_links);
		fprintf(g_pFileOutputLog, "number of physical links =,%d\n", g_number_of_links);

		parser.CloseCSVFile();
	}


	if (parser.OpenCSVFile("input_agent.csv", true))
	{
		int current_agent_id = -1;
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			string agent_id;

			parser.GetValueByFieldName("agent_id", agent_id);

			if (agent_id.length() == 0)  // break for empty line
				break;

			//check correctness of input agent_id
			if (str2int(agent_id)>current_agent_id)
			{
				current_agent_id = str2int(agent_id);
			}
			else
			{
				cout << "Illegal agent id sequence. Program stops." << endl;
				g_ProgramStop();
			}

			TRACE("agent_id = %s\n", agent_id.c_str());
			int agent_type = 0;
			parser.GetValueByFieldName("agent_type", agent_type);
			int external_from_node_id;
			int external_to_node_id;
			int from_node_id;
			int to_node_id;


			if (agent_type == 0) //passenger
			{
				int pax_no = g_number_of_passengers + 1;

				g_internal_agent_no_map[agent_id] = pax_no;
				g_external_passenger_id_map[pax_no + 1] = agent_id;


				if (pax_no >= _MAX_NUMBER_OF_PASSENGERS)
				{
					cout << "Agent+ can handle  " << _MAX_NUMBER_OF_PASSENGERS << "passengers" << endl;
					g_ProgramStop();
				}


				parser.GetValueByFieldName("from_node_id", external_from_node_id);
				parser.GetValueByFieldName("to_node_id", external_to_node_id);

				from_node_id = g_internal_node_no_map[external_from_node_id];
				to_node_id = g_internal_node_no_map[external_to_node_id];

				g_passenger_origin_node[pax_no] = from_node_id;
				g_passenger_destination_node[pax_no] = to_node_id;


				parser.GetValueByFieldName("capacity", g_passenger_demand[pax_no]);
				if (g_passenger_demand[pax_no] < 0)
				{
					cout << "Passenger data must have values in field capacity in file input_agent.csv!" << endl;
					g_ProgramStop();
				}


				parser.GetValueByFieldName("departure_time_start", g_passenger_departure_time_beginning[pax_no]);
				if (g_passenger_departure_time_beginning[pax_no]<0)
				{
					cout << "Illegal departure time input. Program stops." << endl;
					g_ProgramStop();
				}

				int departure_time_window = 0;
				parser.GetValueByFieldName("departure_time_window", departure_time_window);
				if (departure_time_window<0)
				{
					cout << "Illegal departure time window input. Program stops." << endl;
					g_ProgramStop();
				}
				//g_passenger_departure_time_ending[pax_no] = max(0, departure_time_window);
				//g_passenger_departure_time_ending[pax_no] = max(g_passenger_departure_time_ending[pax_no], c);
				g_passenger_departure_time_ending[pax_no] = g_passenger_departure_time_beginning[pax_no] + departure_time_window;

				parser.GetValueByFieldName("arrival_time_start", g_passenger_arrival_time_beginning[pax_no]);
				if (g_passenger_arrival_time_beginning[pax_no]<0)
				{
					cout << "Illegal arrival time input. Program stops." << endl;
					g_ProgramStop();
				}

				int arrival_time_window = 0;
				parser.GetValueByFieldName("arrival_time_window", arrival_time_window);
				if (arrival_time_window<0)
				{
					cout << "Illegal arrival time window input. Program stops." << endl;
					g_ProgramStop();
				}

				g_passenger_arrival_time_ending[pax_no] = g_passenger_arrival_time_beginning[pax_no] + arrival_time_window;

				//g_passenger_arrival_time_ending[pax_no] = max(g_passenger_arrival_time_ending[pax_no], g_passenger_arrival_time_beginning[pax_no]);

				g_number_of_time_intervals = max(g_passenger_arrival_time_ending[pax_no] + 10, g_number_of_time_intervals);

				//adding artificial passenger origin/pickup node id
				int new_artifical_pasenger_origin_id = g_add_new_node(pax_no, g_passenger_departure_time_beginning[pax_no], g_passenger_departure_time_ending[pax_no]);
				g_node_passenger_pickup_flag[new_artifical_pasenger_origin_id] = 1;  // mark this node is a pick-up node

				g_add_new_link(g_passenger_origin_node[pax_no], new_artifical_pasenger_origin_id, pax_no);  // pick up link
				g_add_new_link(new_artifical_pasenger_origin_id, g_passenger_origin_node[pax_no]);

				int new_artifical_pasenger_destination_id = g_add_new_node(pax_no, g_passenger_arrival_time_beginning[pax_no], g_passenger_arrival_time_ending[pax_no]);

				g_passenger_dummy_destination_node[pax_no] = new_artifical_pasenger_destination_id;
				g_add_new_link(g_passenger_destination_node[pax_no], new_artifical_pasenger_destination_id, pax_no *(-1));  // delivery link
				g_add_new_link(new_artifical_pasenger_destination_id, g_passenger_destination_node[pax_no]);

				parser.GetValueByFieldName("base_profit", g_passenger_base_profit[pax_no]);


				g_number_of_passengers++;
			}
			else
			{  // vehicle


				int vehicle_no = g_number_of_vehicles + 1;
				g_external_vehicle_id_map[vehicle_no] = agent_id;


				parser.GetValueByFieldName("from_node_id", external_from_node_id);
				parser.GetValueByFieldName("to_node_id", external_to_node_id);

				from_node_id = g_internal_node_no_map[external_from_node_id];
				to_node_id = g_internal_node_no_map[external_to_node_id];

				g_vehicle_origin_node[vehicle_no] = from_node_id;
				g_vehicle_destination_node[vehicle_no] = to_node_id;

				parser.GetValueByFieldName("departure_time_start", g_vehicle_departure_time_beginning[vehicle_no]);
				int departure_time_window = 0;
				parser.GetValueByFieldName("departure_time_window", departure_time_window);
				g_vehicle_departure_time_ending[vehicle_no] = g_vehicle_departure_time_beginning[vehicle_no] + max(1, departure_time_window);
				g_vehicle_arrival_time_beginning[vehicle_no] = -1;
				parser.GetValueByFieldName("arrival_time_start", g_vehicle_arrival_time_beginning[vehicle_no]);

				if (g_vehicle_arrival_time_beginning[vehicle_no] < 0)
				{
					cout << "Vehicle data must have values in field arrival_time_start in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				int arrival_time_window = -1;
				parser.GetValueByFieldName("arrival_time_window", arrival_time_window);

				if (arrival_time_window < 0)
				{
					cout << "Vehicle data must have values in field arrival_time_window in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				g_vehicle_arrival_time_ending[vehicle_no] = g_vehicle_arrival_time_beginning[vehicle_no] + max(1, arrival_time_window);


				g_number_of_time_intervals = max(g_vehicle_arrival_time_ending[vehicle_no] + 10, g_number_of_time_intervals);

				//			if (g_vehicle_arrival_time_ending[vehicle_no] < g_vehicle_departure_time_beginning[vehicle_no] + 60)  // we should use a shortest path travel time to check. 
				//			{
				//				cout << "warning: Arrival time for vehicle " << vehicle_no << " should be " << g_vehicle_departure_time_beginning[vehicle_no] + 120 << endl;
				//				g_vehicle_arrival_time_ending[vehicle_no] = g_vehicle_departure_time_beginning[vehicle_no] + 60;
				////				g_ProgramStop();
				//			}

				g_activity_node_flag[g_vehicle_origin_node[vehicle_no]] = 1;
				g_activity_node_flag[g_vehicle_destination_node[vehicle_no]] = 1;
				g_activity_node_ending_time[g_vehicle_origin_node[vehicle_no]] = g_vehicle_departure_time_ending[vehicle_no];
				g_activity_node_ending_time[g_vehicle_destination_node[vehicle_no]] = g_vehicle_arrival_time_ending[vehicle_no];


				g_vehicle_capacity[vehicle_no] = -1;



				parser.GetValueByFieldName("capacity", g_vehicle_capacity[vehicle_no]);
				if (g_vehicle_capacity[vehicle_no] < 0)
				{
					cout << "Vehicle data must have values in field capacity in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				parser.GetValueByFieldName("VOIVTT_per_hour", g_VOIVTT_per_hour[vehicle_no]);
				parser.GetValueByFieldName("VOWT_per_hour", g_VOWT_per_hour[vehicle_no]);


				/*if (g_max_vehicle_capacity < g_vehicle_capacity[vehicle_no])
				g_max_vehicle_capacity = g_vehicle_capacity[vehicle_no];*/

				g_number_of_vehicles++;
			}


		}
		parser.CloseCSVFile();
	}

	fprintf(g_pFileOutputLog, "number of passengers=, %d\n", g_number_of_passengers);
	fprintf(g_pFileOutputLog, "number of vehicles =,%d\n", g_number_of_vehicles);

	fprintf(g_pFileDebugLog, "number of passengers=, %d\n", g_number_of_passengers);
	fprintf(g_pFileDebugLog, "number of vehicles =,%d\n", g_number_of_vehicles);

	//beginning for addig virtual vehicles
	g_number_of_physical_vehicles = g_number_of_vehicles;

	for (int p = 1; p <= 1; p++)  /////g_number_of_passengers;;;add only one virtual vehicle
	{
		int	v = (g_number_of_vehicles + 1); //new vehicle id

		g_vehicle_origin_node[v] = g_passenger_origin_node[p];
		g_vehicle_destination_node[v] = g_passenger_origin_node[p];
		g_vehicle_departure_time_beginning[v] = max(0, g_vehicle_departure_time_beginning[p] - 10);
		g_vehicle_departure_time_ending[v] = g_vehicle_departure_time_beginning[v];
		g_vehicle_arrival_time_beginning[v] = g_number_of_time_intervals - 1;
		g_vehicle_arrival_time_ending[v] = g_number_of_time_intervals - 1;
		g_vehicle_capacity[v] = g_passenger_demand[p];
		g_VOIVTT_per_hour[v] = g_dummy_vehicle_cost_per_hour;
		g_VOWT_per_hour[v] = 0;
		g_number_of_vehicles++;
	}

	//end of adding dummy vehicles for each passenger


	//
	for (int i = 0; i < g_number_of_nodes; i++)
	{
		if (g_outbound_node_size[i] > _MAX_NUMBER_OF_OUTBOUND_NODES - 5)
		{
			cout << "Node number " << g_external_node_id_map[i] << " 's outbound node size is larger than predetermined value. Program stops" << endl;
			g_ProgramStop();
		}
	}
	//

	cout << "read " << g_number_of_nodes << " nodes, " << g_number_of_links << " links" << ", " << g_number_of_passengers << " passengers, " << g_number_of_vehicles << "vehicles" << endl;
	
	if (g_debug_out)
	{
		fprintf(g_pFileDebugLog, "network has %d nodes, %d links, %d toll records, %d  passengers, %d vehicles\n",
		g_number_of_nodes, g_number_of_links, g_number_of_toll_records, g_number_of_passengers, g_number_of_vehicles);
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
			fprintf(g_pFileDebugLog, "passenger %d starts from node %d to node %d, start time %d->%d, ending time %d->%d\n",
				p,
				g_external_node_id_map[g_passenger_origin_node[p]],
				g_external_node_id_map[g_passenger_destination_node[p]],
				g_passenger_departure_time_beginning[p],
				g_passenger_departure_time_ending[p],
				g_passenger_arrival_time_beginning[p],
				g_passenger_arrival_time_ending[p]);
		}
		for (int v = 1; v <= g_number_of_vehicles; v++)
		{
			fprintf(g_pFileDebugLog, "vehicle %d starts from node %d to node %d, start time %d->%d, ending time %d->%d\n",
				v,
				g_external_node_id_map[g_vehicle_origin_node[v]],
				g_external_node_id_map[g_vehicle_destination_node[v]],
				g_vehicle_departure_time_beginning[v],
				g_vehicle_departure_time_ending[v],
				g_vehicle_arrival_time_beginning[v],
				g_vehicle_arrival_time_ending[v]);
		}
	}
}



void g_ReadConfiguration()
{

	CCSVParser parser;
	if (parser.OpenCSVFile("input_configuration.csv", true))
	{

		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			string name;

			int node_type;
			int node_id;

			int number_of_threads = 1;
			double X;
			double Y;
			/*parser.GetValueByFieldName("number_of_iterations", g_number_of_LR_iterations);
			parser.GetValueByFieldName("shortest_path_debugging_details", g_shortest_path_debugging_flag);
			parser.GetValueByFieldName("dummy_vehicle_cost_per_hour", g_dummy_vehicle_cost_per_hour);
			parser.GetValueByFieldName("minimum_subgradient_step_size", g_minimum_subgradient_step_size);*/
			parser.GetValueByFieldName("max_number_of_threads_to_be_used", number_of_threads);
			parser.GetValueByFieldName("max_v_in_group", g_max_v_in_group);
			parser.GetValueByFieldName("max_p_in_group", g_max_p_in_group);
			//parser.GetValueByFieldName("vehicle_capacity", g_max_vehicle_capacity);
			parser.GetValueByFieldName("min_finish_task_each_vehicle", g_min_task_accomplish);
			parser.GetValueByFieldName("max_finish_task_each_vehicle", g_max_task_accomplish);

			parser.GetValueByFieldName("memory_for_states", g_memory_for_states);

			if (number_of_threads <= 0)
				number_of_threads = 1;

			if (number_of_threads > omp_get_max_threads())
				number_of_threads = omp_get_max_threads();
			int a = omp_get_max_threads();
			g_number_of_threads = number_of_threads;



			break;  // only the first line
		}
		parser.CloseCSVFile();
	}
}


#pragma endregion 


#pragma region   <service states>

// defintion of passenger service states
class CVR_Service_State  //class for vehicle scheduling service states
{
public:
	int passenger_service_state[_MAX_NUMBER_OF_PASSENGERS]; // code is 0, 1 or 2
	
	int m_boundary_state_flag;	
	int m_passenger_occupancy;//total number of passengers in a vehicle
	int m_task_implement;////total number of tasks the vehicle in the process(include the accompliment) for trans
	int m_task_in_progress;/////total number of passengers in a vehicle   for trans

	CVR_Service_State()
	{
		m_boundary_state_flag = 1;  // true
		m_passenger_occupancy = 0;
		
		m_task_implement = 0;
		m_task_in_progress = 0;
		for (int p = 0; p < _MAX_NUMBER_OF_PASSENGERS; p++)
			passenger_service_state[p] = 0;

	}

	std::vector<int> m_outgoing_state_index_vector;
	std::vector<int> m_outgoing_state_change_service_code_vector;

	bool IsBoundaryState()
	{
	
		if (m_boundary_state_flag == 1)
			return true;
		else
			return false;
	}
	std::string generate_string_key()
	{
		std::string string_key;
		m_task_in_progress = 0; m_task_implement = 0;		
		for (int p = 1; p <= g_number_of_passengers; p++)  // scan all passengers
		{
			
			stringstream s;

			s << "_";
			if (passenger_service_state[p] == 1)
			{
				m_boundary_state_flag = 0;  // if any of passenger service code is 1, then this state is not a boundary state
				m_task_in_progress++;
				s << "1";
			}
			else if (passenger_service_state[p] == 2)  //complete the trip for this passenger p
			{			
				m_task_implement++;
				//s << p;
				s << "2";
			}
			else
			{
				s << " ";
			}

			string converted(s.str());

			string_key += converted;

		}
		return string_key;  //e.g. _ _ _ or _1_2_3 or or _1*_2*_3* or _1_2_3*
	}

	std::string get_serviced_p_str()
	{
		std::string string_key;
		for (int p = 1; p <= g_number_of_passengers; p++)  // scan all passengers
		{
			stringstream s;
			s << "";
			if (passenger_service_state[p] == 2)  //complete the trip for this passenger p
			{				
				s <<"p"<< p <<"_";
			}			

			string converted(s.str());
			string_key += converted;
		}
		return string_key; 
	}

};

std::map<std::string, int> g_service_state_map[_MAX_NUMBER_OF_VEHICLES];  // hash table for mapping unique string key to the numerical state index s

int g_find_service_state_index(int vehicle_stage_index, std::string string_key)
{
	if (g_service_state_map[vehicle_stage_index].find(string_key) != g_service_state_map[vehicle_stage_index].end())
	{
		return g_service_state_map[vehicle_stage_index][string_key];
	}
	else

		return -1;  // not found

}

//std::vector<CVR_Service_State> g_VRStateServiceVector;

std::vector<CVR_Service_State> g_DynamicStateVector[_MAX_NUMBER_OF_VEHICLES];

void g_add_service_states(int group_number, int vehicle_stage_index, int vehicle_index, int parent_state_index, int number_of_passengers)
{

	//CVR_Service_State element = g_VRStateServiceVector[parent_state_index];
	CVR_Service_State element = g_DynamicStateVector[vehicle_stage_index][parent_state_index];

	g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_index_vector.push_back(parent_state_index);  // link my own state index to the parent state
	g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_change_service_code_vector.push_back(0);  // link no change state index to the parent state


	for (int p = 1; p <= number_of_passengers; p++)
	{
		if (g_group_p[group_number][p] == 0)continue; ///only consider the passenger in group p

		if (element.passenger_service_state[p] == 0) // not carrying p yet
		{

			//if (element.m_task_in_progress >= g_max_vehicle_capacity)continue; //////assume that the vehicle capacity is 2
			if ((element.m_task_implement + element.m_task_in_progress) >= g_max_task_accomplish*vehicle_stage_index)continue; //////assume that the vehicle max tasks is 3
			// add pick up state 
			CVR_Service_State new_element;
			int pax_capacity_demand = 0;
			for (int pp = 1; pp <= number_of_passengers; pp++)  // copy vector states to create a new element 
			{
				new_element.passenger_service_state[pp] = element.passenger_service_state[pp];

				if (element.passenger_service_state[pp] == 1)
					pax_capacity_demand += g_passenger_demand[pp];
			}

			if ((pax_capacity_demand + g_passenger_demand[p]) > g_vehicle_capacity[vehicle_index])continue;

			//new_element.m_passenger_occupancy = pax_capacity_demand + g_passenger_demand[p];  // add pax p


				// test capacity 
			new_element.passenger_service_state[p] = 1;  // from 0 to 1

				std::string string_key = new_element.generate_string_key();
				int state_index = g_find_service_state_index(vehicle_stage_index,string_key);  // find if the newly generated state node has been defined already
				if (state_index == -1) // no defined yet
				{
					// add new state
				
					state_index = g_DynamicStateVector[vehicle_stage_index].size();
					g_DynamicStateVector[vehicle_stage_index].push_back(new_element);
					g_service_state_map[vehicle_stage_index][string_key] = state_index;

				}// otherwise do nother

				
				g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_index_vector.push_back(state_index);  // link new state index to the parent state in the state transtion graph
				g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_change_service_code_vector.push_back(p);  // identify the new element is generated due to passenger p


		}
		else  if (element.passenger_service_state[p] == 1) //  ==1 carried
		{
			// add delivery and completition state
			CVR_Service_State new_element;
			int pax_capacity_demand = 0;

			for (int pp = 1; pp <= number_of_passengers; pp++)  // copy vector states
			{
				new_element.passenger_service_state[pp] = element.passenger_service_state[pp];

				/*if (element.passenger_service_state[pp] == 1)
					pax_capacity_demand += g_passenger_demand[pp];*/
			}
			new_element.passenger_service_state[p] = 2;  // from 1 to 2

			//new_element.m_passenger_occupancy = pax_capacity_demand - g_passenger_demand[p];

			std::string string_key = new_element.generate_string_key();
			int state_index = g_find_service_state_index(vehicle_stage_index,string_key);  // find if the newly generated state node has been defined already
			if (state_index == -1)  // no
			{
				// add new state
	
				state_index = g_DynamicStateVector[vehicle_stage_index].size();
				g_DynamicStateVector[vehicle_stage_index].push_back(new_element);
				g_service_state_map[vehicle_stage_index][string_key] = state_index;

			}  //otherwise, do nothing

			g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_index_vector.push_back(state_index);  // link new state index to the parent state
			g_DynamicStateVector[vehicle_stage_index][parent_state_index].m_outgoing_state_change_service_code_vector.push_back((-1)*p);  // link new state index to the parent state
		}

	}
}


class OutgoingServiceState
{
public:
	std::vector<int> m_w2_vector;
};

OutgoingServiceState g_outgoingServiceStateSet[_MAX_NUMBER_OF_STATES];


void g_print_stage_states(int vehicle_stage_index)
{
	fprintf(g_pFileDebugLog, "\n  stage %d, total states number:%d\n", vehicle_stage_index, g_DynamicStateVector[vehicle_stage_index].size());
	if (g_debug_out)
	{
		for (int i = 0; i < g_DynamicStateVector[vehicle_stage_index].size(); i++)
		{
			std::string str = g_DynamicStateVector[vehicle_stage_index][i].generate_string_key();

			fprintf(g_pFileDebugLog, "stage:%d, service state no. %d:(%d,%d) %s; outgoing state list:", vehicle_stage_index, i,
				g_DynamicStateVector[vehicle_stage_index][i].m_task_implement,
				g_DynamicStateVector[vehicle_stage_index][i].m_task_in_progress,
				str.c_str());

			for (int w2 = 0; w2 < g_DynamicStateVector[vehicle_stage_index][i].m_outgoing_state_index_vector.size(); w2++)
			{
				fprintf(g_pFileDebugLog, "%d,", g_DynamicStateVector[vehicle_stage_index][i].m_outgoing_state_index_vector[w2]);
			}

			fprintf(g_pFileDebugLog, "\n");

		}
		fprintf(g_pFileDebugLog, "-----\n");
	}

}
void g_create_stage_service_states(int group_number, int vehicle_stage_index,int vehicle_index, int number_of_passengers, int parent_s_index = -1)
{
	if (vehicle_stage_index == 1)
	{
		CVR_Service_State route_element; // 0000000000 restricted in the construction function by setting passenger_service_state = 0 for each passenger
		std::string string_key = route_element.generate_string_key();
		g_service_state_map[vehicle_stage_index][string_key] = 0;// 0 is the root node index
		g_DynamicStateVector[vehicle_stage_index].push_back(route_element);
	}

	

	////////creat states.
	int scan_state_index = 0;
	
	while (g_DynamicStateVector[vehicle_stage_index].size() < _MAX_NUMBER_OF_STATES &&
		scan_state_index< g_DynamicStateVector[vehicle_stage_index].size() &&
		scan_state_index< _MAX_NUMBER_OF_STATES)
	{
		g_add_service_states(group_number, vehicle_stage_index, vehicle_index, scan_state_index, number_of_passengers);
		scan_state_index++;
	}	

}


#pragma endregion


#pragma region <memory allocate & free>


//parallel computing 
float**** lp_state_node_label_cost = NULL;
int**** lp_state_node_predecessor = NULL;
int**** lp_state_time_predecessor = NULL;
int**** lp_state_carrying_predecessor = NULL;

//state vertex label cost: la as local assignment variables 
float**** la_state_node_label_cost = NULL;

int**** la_state_node_predecessor = NULL;
int**** la_state_time_predecessor = NULL;
int**** la_state_service_predecessor = NULL;
int**** la_state_vstage_predecessor = NULL;


float** g_node_to_node_shorest_travel_time = NULL;


float*** g_v_arc_cost = NULL;
float*** g_v_to_node_cost_used_for_upper_bound = NULL;
float*** g_v_vertex_waiting_cost = NULL;


void g_allocate_memory_DP(int number_of_processors)
{

	int number_of_states =0;
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_links = g_number_of_links + 1;
	int number_of_time_intervals = g_number_of_time_intervals + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;


	g_v_arc_cost = Allocate3DDynamicArray<float>(number_of_vehicles, number_of_links, number_of_time_intervals);
	g_v_to_node_cost_used_for_upper_bound = Allocate3DDynamicArray<float>(number_of_vehicles, number_of_nodes, number_of_time_intervals);
	g_v_vertex_waiting_cost = Allocate3DDynamicArray<float>(number_of_vehicles, number_of_nodes, number_of_time_intervals);



	if (g_number_of_time_intervals >= _MAX_NUMBER_OF_TIME_INTERVALS)
	{
		cout << "Program configuration issue: _MAX_NUMBER_OF_TIME_INTERVALS= " << _MAX_NUMBER_OF_TIME_INTERVALS << "is less than g_number_of_time_intervals = " << g_number_of_time_intervals << endl;
		cout << "Please contact developer." << endl;
		g_ProgramStop();
	}

	
	//number_of_processors = g_number_of_threads;
	//
	//lp_state_node_label_cost = Allocate4DDynamicArray<float>(number_of_processors,number_of_nodes, number_of_time_intervals, number_of_states);
	//lp_state_node_predecessor = Allocate4DDynamicArray<int>(number_of_processors, number_of_nodes, number_of_time_intervals, number_of_states);
	//lp_state_time_predecessor = Allocate4DDynamicArray<int>(number_of_processors, number_of_nodes, number_of_time_intervals, number_of_states);
	//lp_state_carrying_predecessor = Allocate4DDynamicArray<int>(number_of_processors, number_of_nodes, number_of_time_intervals, number_of_states);

	g_node_to_node_shorest_travel_time = AllocateDynamicArray<float>(number_of_nodes, number_of_nodes, 999);

	number_of_vehicles =g_max_v_in_group +1;

	//number_of_states = min(pow(3, g_max_p_in_group) + 1, _MAX_NUMBER_OF_STATES); ///
	number_of_states = g_memory_for_states+1;///debug
	number_of_nodes = g_map_groupNode_to_globalNode[0][0]+1;
	cout << "allocate memory number of service states = " << number_of_states << ",nodes =" << number_of_nodes<< endl;
	cout << "  memory acount:v*t*s*i = " << number_of_vehicles << "*" << number_of_time_intervals << "*" << number_of_states << "*" << number_of_nodes <<"="<<
		number_of_vehicles*number_of_time_intervals *number_of_states *number_of_nodes << endl;

	la_state_node_label_cost = Allocate4DDynamicArray<float>(number_of_vehicles, number_of_time_intervals, number_of_states, number_of_nodes);
	la_state_node_predecessor = Allocate4DDynamicArray<int>(number_of_vehicles, number_of_time_intervals, number_of_states, number_of_nodes);
	la_state_time_predecessor = Allocate4DDynamicArray<int>(number_of_vehicles, number_of_time_intervals, number_of_states, number_of_nodes);
	la_state_service_predecessor = Allocate4DDynamicArray<int>(number_of_vehicles, number_of_time_intervals, number_of_states, number_of_nodes);
	la_state_vstage_predecessor = Allocate4DDynamicArray<int>(number_of_vehicles, number_of_time_intervals, number_of_states, number_of_nodes);
}


void g_free_memory_DP(int number_of_processors)
{
	int number_of_states = 0;
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_links = g_number_of_links + 1;
	int number_of_time_intervals = g_number_of_time_intervals + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;

	
	
	Deallocate3DDynamicArray<float>(g_v_arc_cost, number_of_vehicles, number_of_links);
	Deallocate3DDynamicArray<float>(g_v_to_node_cost_used_for_upper_bound, number_of_vehicles, number_of_nodes);
	Deallocate3DDynamicArray<float>(g_v_vertex_waiting_cost, number_of_vehicles, number_of_nodes);

	/*number_of_processors = g_number_of_threads;

	Deallocate4DDynamicArray<float>(lp_state_node_label_cost, number_of_processors, number_of_nodes, number_of_time_intervals);
	Deallocate4DDynamicArray<int>(lp_state_node_predecessor, number_of_processors, number_of_nodes, number_of_time_intervals);
	Deallocate4DDynamicArray<int>(lp_state_time_predecessor, number_of_processors,  number_of_nodes, number_of_time_intervals);
	Deallocate4DDynamicArray<int>(lp_state_carrying_predecessor, number_of_processors, number_of_nodes, number_of_time_intervals);*/


	DeallocateDynamicArray<float>(g_node_to_node_shorest_travel_time, number_of_nodes, number_of_nodes);
	

	number_of_vehicles = g_max_v_in_group + 1;
	//number_of_states = min(pow(3, g_max_p_in_group) + 1, _MAX_NUMBER_OF_STATES);
	number_of_states = g_memory_for_states+1;///debug
	number_of_nodes = g_map_groupNode_to_globalNode[0][0] + 1;

	Deallocate4DDynamicArray<float>(la_state_node_label_cost, number_of_vehicles, number_of_time_intervals, number_of_states);
	Deallocate4DDynamicArray<int>(la_state_node_predecessor, number_of_vehicles, number_of_time_intervals, number_of_states);
	Deallocate4DDynamicArray<int>(la_state_time_predecessor, number_of_vehicles, number_of_time_intervals, number_of_states);
	Deallocate4DDynamicArray<int>(la_state_service_predecessor, number_of_vehicles, number_of_time_intervals, number_of_states);
	Deallocate4DDynamicArray<int>(la_state_vstage_predecessor, number_of_vehicles, number_of_time_intervals, number_of_states);


}

void g_allocate_memory_travel_time(int number_of_processors)
{
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;

	g_node_to_node_shorest_travel_time = AllocateDynamicArray<float>(number_of_nodes, number_of_nodes, 999);
	g_vehicle_origin_based_node_travel_time = AllocateDynamicArray<float>(number_of_vehicles, number_of_nodes, 0);
	g_vehicle_destination_based_node_travel_time = AllocateDynamicArray<float>(number_of_vehicles, number_of_nodes, 0);

}

void g_free_memory_travel_time(int number_of_processors)
{
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;

	exit(0);  // replying on OS to release memory

	DeallocateDynamicArray<float>(g_node_to_node_shorest_travel_time, number_of_nodes, number_of_nodes);
	DeallocateDynamicArray<float>(g_vehicle_origin_based_node_travel_time, number_of_vehicles, number_of_nodes);
	DeallocateDynamicArray<float>(g_vehicle_destination_based_node_travel_time, number_of_vehicles, number_of_nodes);

}

#pragma endregion



#pragma region <main function>




float g_output_route_for_final_states(int group_number,int state_index, int final_last_s, int final_last_v_stage,int stage_map_to_v[])
{
	double total_cost = _MAX_LABEL_COST;

	int reversed_path_vstage_sequence[_MAX_NUMBER_OF_STEP_INTERVALS];
	int reversed_path_time_sequence[_MAX_NUMBER_OF_STEP_INTERVALS];
	int reversed_path_state_sequence[_MAX_NUMBER_OF_STEP_INTERVALS];
	int reversed_path_node_sequence[_MAX_NUMBER_OF_STEP_INTERVALS];
	float reversed_path_cost_sequence[_MAX_NUMBER_OF_STEP_INTERVALS];

	int last_s = final_last_s;  // 1 for all serviced state node
	int last_v_stage = final_last_v_stage;
	int last_v = stage_map_to_v[last_v_stage];
	int destination_node = g_vehicle_destination_node[last_v];
	int arrival_time = g_vehicle_arrival_time_ending[last_v];
	
	int destination_node_group = g_map_globalNode_to_groupNode[group_number][destination_node];
	total_cost = la_state_node_label_cost[last_v_stage][arrival_time][last_s][destination_node_group];



	// step 2: backtrack to the origin (based on node and time predecessors)
	int	node_size = 0;
	reversed_path_node_sequence[node_size] = destination_node_group;//record the first node backward, destination node
	reversed_path_time_sequence[node_size] = arrival_time;
	reversed_path_state_sequence[node_size] = last_s;
	reversed_path_vstage_sequence[node_size] = last_v_stage;
	reversed_path_cost_sequence[node_size] = la_state_node_label_cost[last_v_stage][arrival_time][last_s][destination_node_group];


	node_size++;

	int pred_node = la_state_node_predecessor[last_v_stage][arrival_time][last_s][destination_node_group];
	int pred_time = la_state_time_predecessor[last_v_stage][arrival_time][last_s][destination_node_group];
	int pred_state = la_state_service_predecessor[last_v_stage][arrival_time][last_s][destination_node_group];
	int pred_v_stage = la_state_vstage_predecessor[last_v_stage][arrival_time][last_s][destination_node_group];
	
	int previous_v_stage = pred_v_stage;
	int vehicle_route_part_a_start_index[_MAX_NUMBER_OF_VEHICLES] = { 0 };
	int vehicle_route_part_b_start_index[_MAX_NUMBER_OF_VEHICLES] = { 0 };
	int vehicle_route_part_c_start_index[_MAX_NUMBER_OF_VEHICLES] = { 0 };

	while (pred_node != -1)// && node_size < _MAX_NUMBER_OF_TIME_INTERVALS) // scan backward in the predessor array of the shortest path calculation results
	{
		reversed_path_node_sequence[node_size] = pred_node;
		reversed_path_time_sequence[node_size] = pred_time;
		reversed_path_state_sequence[node_size] = pred_state;
		reversed_path_vstage_sequence[node_size] = pred_v_stage;
		reversed_path_cost_sequence[node_size] = la_state_node_label_cost[pred_v_stage][pred_time][pred_state][pred_node];

		if (previous_v_stage == pred_v_stage)
		{
			int active_passerger_id = g_node_passenger_id[g_map_groupNode_to_globalNode[group_number][pred_node]];
			if (vehicle_route_part_c_start_index[pred_v_stage] == 0 && active_passerger_id != 0)
			{
				vehicle_route_part_c_start_index[pred_v_stage] = node_size;
				g_vehicle_latest_deliver_passenger[stage_map_to_v[pred_v_stage]] = active_passerger_id;
				g_vehicle_latest_deliver_time[stage_map_to_v[pred_v_stage]] = pred_time+1;

			}
		}
		else
		{
			vehicle_route_part_a_start_index[pred_v_stage] = node_size;
		}

		node_size++;

		//record current values of node and time predecessors, and update PredNode and PredTime

		int pred_node_record = pred_node;
		int pred_time_record = pred_time;
		int pred_state_record = pred_state;
		int pred_vstage_record = pred_v_stage;


		pred_node = la_state_node_predecessor[pred_vstage_record][pred_time_record][pred_state_record][pred_node_record];
		pred_time = la_state_time_predecessor[pred_vstage_record][pred_time_record][pred_state_record][pred_node_record];
		pred_state = la_state_service_predecessor[pred_vstage_record][pred_time_record][pred_state_record][pred_node_record];
		pred_v_stage = la_state_vstage_predecessor[pred_vstage_record][pred_time_record][pred_state_record][pred_node_record];

		previous_v_stage = pred_v_stage;

	}

	//reverse the node sequence 
	
	
	float prev_cost = -1; int prev_state = -1; int prev_v = -1;
	string route_type = "a";
	for (int n = 0; n < node_size; n++)
	{

		int now_path_node = reversed_path_node_sequence[node_size - n - 1];
		int now_path_time = reversed_path_time_sequence[node_size - n - 1];
		int now_path_state = reversed_path_state_sequence[node_size - n - 1];
		int now_path_vstage = reversed_path_vstage_sequence[node_size - n - 1];
		int now_path_cost = reversed_path_cost_sequence[node_size - n - 1];
		///////////

		int node = now_path_node;
		int node_global = g_map_groupNode_to_globalNode[group_number][node];

		int vstage = now_path_vstage;
		int v = stage_map_to_v[vstage];
		int p = g_node_passenger_id[node];
		if (node >= 0 && p >= 1)
			g_vehicle_serving_passenger_matrix[v][p] = 1;

		/////////////print
		std::string str = g_DynamicStateVector[vstage][now_path_state].generate_string_key();
		float cost = now_path_cost;
		int state = now_path_state;

		int v_active = stage_map_to_v[now_path_vstage];
		int p_active = g_node_passenger_id[node_global];
		
		////////route type
		if (node_size - n - 1 <= vehicle_route_part_c_start_index[now_path_vstage])
		{
			route_type = "c";
			if (node_size - n - 1 == vehicle_route_part_c_start_index[now_path_vstage])route_type = "c0";
		}
		else
		{	
			route_type = "a";
			if (vehicle_route_part_b_start_index[now_path_vstage]!=0 && n >= vehicle_route_part_b_start_index[now_path_vstage])
				route_type = "b";
			if (vehicle_route_part_b_start_index[now_path_vstage] == 0 && p_active != 0)
			{
				vehicle_route_part_b_start_index[now_path_vstage] = n; route_type = "b";

				g_vehicle_earliest_pickup_passenger[stage_map_to_v[now_path_vstage]] = p_active;
				g_vehicle_earliest_pickup_time[stage_map_to_v[now_path_vstage]] = now_path_time-1;
			}
		}
		

		if (p_active != 0 || fabs(cost - prev_cost) > 0.0001 || prev_v!=v_active)
		{
			if (p_active != 0 && state == prev_state)continue;			
			///////////////
			if (g_debug_out)
			{
				fprintf(g_pFileDebugLog, "\group %d,index %d,stage %d, Vehicle %d,[g,s]=[%d,%d]: time = %d,p=%d, node = %d, state[%d] = %s, cost = %f,routeType=%s\n",
					group_number,
					n,
					now_path_vstage, v_active,
					group_number, state_index,
					now_path_time, p_active,
					g_external_node_id_map[node_global],
					now_path_state,
					str.c_str(),
					cost,
					route_type.c_str());
			}

			//if (l_path_node_sequence[n] <= g_number_of_physical_nodes)
				fprintf(g_pFile_All_Routes, "\%d,%d,%d,%d,%d,%d,%f,%s\n",
				group_number,state_index,
				v_active,p_active,
				now_path_time,
				g_external_node_id_map[node_global],
				cost,
				route_type.c_str());

		}
		prev_cost = cost;
		prev_state = state;
		prev_v = v_active;
	}


	return total_cost;
}

////main funciton
//parallel computing version///not now
void g_integrated_assignment_routing_dynamic_programming(int group_number)
{

	// step 1: Initialization for all nodes
	for (int vs = 0; vs < g_max_v_in_group+1; vs++) //Initialization for all nodes
	{
		//if (g_group_v[group_number][v] == 0)continue;

			for (int t = 0; t < g_number_of_time_intervals; t++)
			{
				for (int s = 0; s <g_memory_for_states+1; s++)
				{
					for (int i = 0; i <= g_map_groupNode_to_globalNode[group_number][0]; i++) //Initialization for all nodes
					{
						la_state_node_label_cost[vs][t][s][i] = _MAX_LABEL_COST;
						la_state_node_predecessor[vs][t][s][i] = -1;  // pointer to previous NODE INDEX from the current label at current node and time
						la_state_time_predecessor[vs][t][s][i] = -1;  // pointer to previous TIME INDEX from the current label at current node and time
						la_state_service_predecessor[vs][t][s][i] = -1;
						la_state_vstage_predecessor[vs][t][s][i] = -1;
					}
				}
			}		
	}

	//step 2: Initialization for origin node at the preferred departure time, at departure time
	int last_s ;  // for final states
	int last_v_stage;   // for final vehicle stage

	int v_stage = 0;
	int v_start = 0;
	for (int v_in = 1; v_in <= g_number_of_vehicles;v_in++)
	if (g_group_v[group_number][v_in] == 1)
	{
		v_start = v_in; break;
	}

	int v_map_to_stage[_MAX_NUMBER_OF_VEHICLES] = { 0 };
	int stage_map_to_v[_MAX_NUMBER_OF_VEHICLES] = { 0 };

	int s0 = 0;  // start from empty
	int origin_node = g_vehicle_origin_node[v_start];
	int departure_time_beginning = g_vehicle_departure_time_beginning[v_start];

	//la_state_node_label_cost[v_start][departure_time_beginning][s0][origin_node] = 0;
	la_state_node_label_cost[1][departure_time_beginning][s0][g_map_globalNode_to_groupNode[group_number][origin_node]] = 0;

	for (int v = 1; v <= g_number_of_physical_vehicles; v++) // 1st loop for vehicle
	{

		if (g_group_v[group_number][v] == 0)continue;
		v_stage++;
		v_map_to_stage[v] = v_stage;
		stage_map_to_v[v_stage] = v;
		//////////////////////////
		int arrival_time_ending = g_vehicle_arrival_time_ending[v];
		departure_time_beginning = g_vehicle_departure_time_beginning[v];
		
		int vehicle_capacity = g_vehicle_capacity[v];
	
		for (int t = departure_time_beginning; t <= arrival_time_ending; t++)  //2nd loop: time
		{
			if (t % 20 == 0)
			{
					cout << " vehicle " << v << " is scanning time " << t << "..." << endl;
			}
			for (int s1 = 0; s1 < g_DynamicStateVector[v_stage].size(); s1++)  // 3rd loop, service state
			{

				if (g_DynamicStateVector[v_stage][s1].m_passenger_occupancy > vehicle_capacity) // skip all states exceeding vehicle capacity
					continue;


			for (int link = 0; link < g_number_of_links; link++)  // 4th loop for each link (i,j)
			{

				if (g_group_link[group_number][link] == 0)continue;////only consider the partion network

				int from_node = g_link_from_node_id[link];  //i
				int to_node = g_link_to_node_id[link];  // j

				int to_node_group = g_map_globalNode_to_groupNode[group_number][to_node];/////attention: only la*** variable use this
				int from_node_group = g_map_globalNode_to_groupNode[group_number][from_node];/////attention: only la*** variable use this

				int upstream_p = g_node_passenger_id[from_node];
				int downsteram_p = g_node_passenger_id[to_node];

				int travel_time = g_link_free_flow_travel_time[link];

				if (la_state_node_label_cost[v_stage][t][s1][from_node_group] < _MAX_LABEL_COST - 1)  // for feasible time-space point only
					{
						for (int s2_index = 0; s2_index < g_DynamicStateVector[v_stage][s1].m_outgoing_state_index_vector.size(); s2_index++)
						{
							if (g_DynamicStateVector[v_stage][s1].m_outgoing_state_change_service_code_vector[s2_index] != g_link_service_code[link])  //0,  +p or -p
								continue;

					

							int s2 = g_DynamicStateVector[v_stage][s1].m_outgoing_state_index_vector[s2_index];

							if (g_DynamicStateVector[v_stage][s2].m_passenger_occupancy > vehicle_capacity)
								continue;

							// part 1: link based update
							int new_to_node_arrival_time = min(t + travel_time, g_number_of_time_intervals - 1);

							if (g_node_passenger_id[to_node] >= 1 && g_activity_node_starting_time[to_node] >= 0 && g_activity_node_ending_time[to_node] >= 0)
								// passegner activity node: origin or destination
							{

								if (new_to_node_arrival_time < g_activity_node_starting_time[to_node]
									|| new_to_node_arrival_time > g_activity_node_ending_time[to_node])
								{
									// skip scanning when the destination nodes arrival time is out of time window
									continue;
								}
							}


							//					if (g_shortest_path_debugging_flag)
							//						fprintf(g_pFileDebugLog, "SP: checking from node %d, to node %d at time %d, FFTT = %d\n",
							//					from_node, to_node, new_to_node_arrival_time,  g_link_free_flow_travel_time[link_no]);
							float temporary_label_cost = la_state_node_label_cost[v_stage][t][s1][from_node_group] + g_v_arc_cost[v][link][t];
							
							//if (v > g_number_of_physical_vehicles)temporary_label_cost += 20;///for virtual vehicle price

							if (temporary_label_cost < la_state_node_label_cost[v_stage][new_to_node_arrival_time][s2][to_node_group]) // we only compare cost at the downstream node ToID at the new arrival time t
							{

								/*if (g_shortest_path_debugging_flag)
								{
									fprintf(g_pFileDebugLog, "DP: updating node: %d from time %d to time %d, current cost: %.2f, from cost %.2f ->%.2f\n",
										to_node, t, new_to_node_arrival_time,
										lp_state_node_label_cost[p][from_node][t][w2],
										lp_state_node_label_cost[p][to_node][new_to_node_arrival_time][w2], temporary_label_cost);
								}*/

								/*fprintf(g_pFileDebugLog, "vehicle %d: State index: %d->%d,  time: %d->%d, Link: %d->%d, cost:%f\n",
									v,
									s1,s2,
									t, new_to_node_arrival_time,
									g_external_node_id_map[from_node],
									g_external_node_id_map[to_node],
									temporary_label_cost);*/


								// update cost label and node/time predecessor
								
								la_state_node_label_cost[v_stage][new_to_node_arrival_time][s2][to_node_group] = temporary_label_cost;
								la_state_node_predecessor[v_stage][new_to_node_arrival_time][s2][to_node_group] = from_node_group;  // pointer to previous NODE INDEX from the current label at current node and time
								la_state_time_predecessor[v_stage][new_to_node_arrival_time][s2][to_node_group] = t;  // pointer to previous TIME INDEX from the current label at current node and time
								la_state_service_predecessor[v_stage][new_to_node_arrival_time][s2][to_node_group] = s1;
								la_state_vstage_predecessor[v_stage][new_to_node_arrival_time][s2][to_node_group] = v_stage;
							}
							// part 2: same node based update for waiting arcs

							if (s2 == s1) // for the same state
							{

								new_to_node_arrival_time = min(t + 1, g_number_of_time_intervals - 1);

								//					if (g_shortest_path_debugging_flag)
								//						fprintf(g_pFileDebugLog, "SP: checking from node %d, to node %d at time %d, FFTT = %d\n",
								//					from_node, to_node, new_to_node_arrival_time,  g_link_free_flow_travel_time[link_no]);
								temporary_label_cost = la_state_node_label_cost[v_stage][t][s1][from_node_group] + +g_v_vertex_waiting_cost[v][from_node][t];

	
								if (temporary_label_cost < la_state_node_label_cost[v_stage][new_to_node_arrival_time][s1][from_node_group]) // we only compare cost at the downstream node ToID at the new arrival time t
								{

									/* fprintf(g_pFileDebugLog, "vehicle %d: State index: %d,  time: %d, wait node: %d, cost:%f\n",
										 v,
										 s1,
										 t,
										 g_external_node_id_map[from_node],
										 temporary_label_cost);*/

									// update cost label and node/time predecessor

									la_state_node_label_cost[v_stage][new_to_node_arrival_time][s1][from_node_group] = temporary_label_cost;
									la_state_node_predecessor[v_stage][new_to_node_arrival_time][s1][from_node_group] = from_node_group;  // pointer to previous NODE INDEX from the current label at current node and time
									la_state_time_predecessor[v_stage][new_to_node_arrival_time][s1][from_node_group] = t;  // pointer to previous TIME INDEX from the current label at current node and time
									la_state_service_predecessor[v_stage][new_to_node_arrival_time][s1][from_node_group] = s1;
									la_state_vstage_predecessor[v_stage][new_to_node_arrival_time][s1][from_node_group] = v_stage;
								 }
							}

						}
					}  // feasible vertex label cost
				}  // for all links

			} // for all states
			} // for all time t

		//////////////////////////////////////////////////////////////
		// RACE relay section from v_stage->v_stage+1
		/////////////////////////////////////////////////////
		bool b_is_last_stage = false;int v_next = 0;
		for (int v_in = v + 1; v_in <= g_number_of_vehicles; v_in++)
		{
			if (g_group_v[group_number][v_in] == 1){ v_next = v_in; break; }
		}
		if (v_next == g_number_of_physical_vehicles + 1)b_is_last_stage = true;
		

		int task_total_number = 0;
		for (int p_in = 1; p_in <= g_number_of_passengers; p_in++)
		{
			if (g_group_p[group_number][p_in] == 1){ task_total_number++; }
		}
		///////////////////////////////////////////////////////
		
			
		int m_seed_states[_MAX_NUMBER_OF_STATES] = { 0 };
		for (int s_boundary = 0; s_boundary < g_DynamicStateVector[v_stage].size(); s_boundary++)  //  service state
		{
		// boundary states only
			if (g_DynamicStateVector[v_stage][s_boundary].IsBoundaryState() == false)continue;
			/*if (g_DynamicStateVector[v_stage][s_boundary].IsBoundaryState() == true)*/
			{ 				

				std::string str = g_DynamicStateVector[v_stage][s_boundary].generate_string_key();

				int t1 = g_vehicle_arrival_time_ending[v];
				int t2 = g_vehicle_departure_time_beginning[v_next];
				int node1 = g_vehicle_destination_node[v];
				int node2 = g_vehicle_origin_node[v_next] ;
				int node1_group = g_map_globalNode_to_groupNode[group_number][node1];
				int node2_group = g_map_globalNode_to_groupNode[group_number][node2];

				float boundaryState_label_cost = la_state_node_label_cost[v_stage][t1][s_boundary][node1_group];

				if (boundaryState_label_cost == _MAX_LABEL_COST)continue;													
				//////////skip the un reachable boundary states						
				//if (boundaryState_label_cost >= 100 * (v_stage + 1))continue;//////not allow to many deadheading
				/////skip not enough task boundary states
				if (g_DynamicStateVector[v_stage][s_boundary].m_task_implement < g_min_task_accomplish*v_stage)continue;/////each v should finish min_task_number

				m_seed_states[++m_seed_states[0]] = s_boundary;
					
				if (g_debug_out)
				{
					fprintf(g_pFileDebugLog, "Relay stage %d(%d)-%d(%d): relaying state no. %d->%d %s, with label cost %f\n",
						v_stage, v, v_stage + 1, v_next,
						s_boundary,
						m_seed_states[0] - 1,
						str.c_str(),
						la_state_node_label_cost[v_stage][t1][s_boundary][node1_group]);
				}

				if (!b_is_last_stage)
				{
					int new_s = m_seed_states[0]-1;
					//new_s = g_find_service_state_index(v_stage + 1, str);  // find if the newly generated state node has been defined already
						
					la_state_node_label_cost[v_stage + 1][t2][new_s][node2_group] = la_state_node_label_cost[v_stage][t1][s_boundary][node1_group];
					la_state_node_predecessor[v_stage + 1][t2][new_s][node2_group] = node1_group;// la_state_node_predecessor[v_stage][t1][s_boundary][node1_group];
					la_state_time_predecessor[v_stage + 1][t2][new_s][node2_group] = t1;// la_state_time_predecessor[v_stage][t1][s_boundary][node1_group];
					la_state_service_predecessor[v_stage + 1][t2][new_s][node2_group] = s_boundary;// la_state_service_predecessor[v_stage][t1][s_boundary][node1_group];
					la_state_vstage_predecessor[v_stage + 1][t2][new_s][node2_group] = v_stage;

					/*if (new_s==6)
					g_output_route_for_final_states(group_number, 0, s_boundary, v_stage, stage_map_to_v);///degug*/
				}
				else
					//////////for c(g,s),a(g,p,s),or x(g,s)					
					/////////for the last stage(no virtual vehicle), computer each boundary states for set partition problem parameters
				{

					if (g_DynamicStateVector[v_stage][s_boundary].m_task_implement < task_total_number)continue;/////only consider near finish number
					/////////////////
					g_parameter_c[group_number][(int)++g_parameter_c[group_number][0]] = boundaryState_label_cost;
					g_parameter_a[group_number][0][0]++;
					for (int p = 1; p <= g_number_of_passengers; p++)
					{
						if (g_group_p[group_number][p] == 1 && g_DynamicStateVector[v_stage][s_boundary].passenger_service_state[p] == 2)
							g_parameter_a[group_number][p][g_parameter_a[group_number][0][0]] = 1;
						else
							g_parameter_a[group_number][p][g_parameter_a[group_number][0][0]] = 0;
					}
					///////////////////////put each route into trans.all_routes.csv.file
					int state_order_number = g_parameter_c[group_number][0];
					
					g_output_route_for_final_states(group_number, state_order_number, s_boundary, v_stage, stage_map_to_v);

					///////print for trans_map_g_s_str.csv
					///g,s_index,state_str,p
					std::string strP = g_DynamicStateVector[v_stage][s_boundary].get_serviced_p_str();
					fprintf(g_pFile_Map_GS_Str, "%d,%d,%s,%s\n",
						group_number, state_order_number, str.c_str(),strP.c_str());
				}					
			}				
		}
					
		////dynamic generate states
		if (!b_is_last_stage)
		{
			////generate from m_seed_states[]
			for (int i_seed_index = 1; i_seed_index <= m_seed_states[0]; i_seed_index++)
			{
				int parent_state_index = m_seed_states[i_seed_index];
				CVR_Service_State element = g_DynamicStateVector[v_stage][parent_state_index];
				
				CVR_Service_State seed_element;						
				for (int pp = 1; pp <= g_number_of_passengers; pp++)  // copy vector states to create a new element 
					seed_element.passenger_service_state[pp] = element.passenger_service_state[pp];				

				std::string string_key = seed_element.generate_string_key();

				g_service_state_map[v_stage + 1][string_key] = i_seed_index-1;
				g_DynamicStateVector[v_stage + 1].push_back(seed_element);
			}
			////////////
			////// create states
			cout << "\n--group:"<< group_number<<"-- create stage "<< v_stage+1<<" service states....";			

			g_create_stage_service_states(group_number, v_stage + 1,v_next, g_number_of_passengers, 0);//////create next stage's states.

			cout << "\n--states number:" << g_DynamicStateVector[v_stage + 1].size() << endl;
			g_print_stage_states(v_stage + 1);/////print stage's states.

		}////////end of dynamic general next stage's states.
	}  // for different vehicle-stages		
}


#pragma endregion



#pragma region "shortest path"


int g_ListFront;
int g_ListTail;
int g_SENodeList[_MAX_NUMBER_OF_NODES];

// SEList: Scan List implementation: the reason for not using STL-like template is to avoid overhead associated pointer allocation/deallocation
void SEList_clear()
{
	g_ListFront = -1;
	g_ListTail = -1;
}

void SEList_push_front(int node)
{
	if (g_ListFront == -1)  // start from empty
	{
		g_SENodeList[node] = -1;
		g_ListFront = node;
		g_ListTail = node;
	}
	else
	{
		g_SENodeList[node] = g_ListFront;
		g_ListFront = node;
	}

}
void SEList_push_back(int node)
{
	if (g_ListFront == -1)  // start from empty
	{
		g_ListFront = node;
		g_ListTail = node;
		g_SENodeList[node] = -1;
	}
	else
	{
		g_SENodeList[g_ListTail] = node;
		g_SENodeList[node] = -1;
		g_ListTail = node;
	}
}

bool SEList_empty()
{
	return(g_ListFront == -1);
}

int SEList_front()
{
	return g_ListFront;
}

void SEList_pop_front()
{
	int tempFront = g_ListFront;
	g_ListFront = g_SENodeList[g_ListFront];
	g_SENodeList[tempFront] = -1;
}

int g_node_status_static_array[_MAX_NUMBER_OF_NODES];

float g_node_label_earliest_arrival_time[_MAX_NUMBER_OF_NODES];
int g_node_static_predecessor[_MAX_NUMBER_OF_NODES];


float g_optimal_label_correcting(int origin_node, int departure_time)
	// time-dependent label correcting algorithm with double queue implementation
{
	int internal_debug_flag = 0;

	float total_cost = _MAX_LABEL_COST;
	if (g_outbound_node_size[origin_node] == 0)
	{
		return _MAX_LABEL_COST;
	}

	for (int i = 0; i < g_number_of_nodes; i++) //Initialization for all nodes
	{
		g_node_status_static_array[i] = 0;  // not scanned
		g_node_label_earliest_arrival_time[i] = _MAX_LABEL_COST;
		g_node_static_predecessor[i] = -1;  // pointer to previous NODE INDEX from the current label at current node and time
	}

	//Initialization for origin node at the preferred departure time, at departure time, cost = 0, otherwise, the delay at origin node

	g_node_label_earliest_arrival_time[origin_node] = departure_time;


	SEList_clear();
	SEList_push_back(origin_node);


	while (!SEList_empty())
	{
		int from_node = SEList_front();//pop a node FromID for scanning

		SEList_pop_front();  // remove current node FromID from the SE list
		g_node_status_static_array[from_node] = 0;


		if (g_shortest_path_debugging_flag)
			fprintf(g_pFileDebugLog, "SP: SE node: %d\n", from_node);

		//scan all outbound nodes of the current node
		for (int i = 0; i < g_outbound_node_size[from_node]; i++)  // for each link (i,j) belong A(i)
		{
			int link_no = g_outbound_link_no[from_node][i];
			int to_node = g_outbound_node_id[from_node][i];

			if (to_node==3)
			{
				int q = 0;
			}

			bool  b_node_updated = false;


			int new_to_node_arrival_time = g_node_label_earliest_arrival_time[from_node] + g_link_free_flow_travel_time[link_no];

			//					if (g_shortest_path_debugging_flag)
			//						fprintf(g_pFileDebugLog, "SP: checking from node %d, to node %d at time %d, FFTT = %d\n",
			//					from_node, to_node, new_to_node_arrival_time,  g_link_free_flow_travel_time[link_no]);


			if (new_to_node_arrival_time < g_node_label_earliest_arrival_time[to_node]) // we only compare cost at the downstream node ToID at the new arrival time t
			{

				//if (g_shortest_path_debugging_flag)
				//	fprintf(g_pFileDebugLog, "SP: updating node: %d from time %d to time %d, current cost: %.2f, from cost %.2f ->%.2f\n",
				//	to_node, t, new_to_node_arrival_time,
				//	g_node_label_cost[from_node][t],
				//	g_node_label_cost[to_node][new_to_node_arrival_time], temporary_label_cost);

				// update cost label and node/time predecessor

				g_node_label_earliest_arrival_time[to_node] = new_to_node_arrival_time;
				g_node_static_predecessor[to_node] = from_node;  // pointer to previous physical NODE INDEX from the current label at current node and time

				b_node_updated = true;


				if (g_node_status_static_array[to_node] != 1)
				{
					/*if (g_shortest_path_debugging_flag)
						fprintf(g_pFileDebugLog, "SP: add node %d into SE List\n",
						to_node);*/

					SEList_push_back(to_node);				
					g_node_status_static_array[to_node] = 1;
				}
			}

		}
	}

	for (int i = 0; i <= g_number_of_nodes; i++) //Initialization for all nodes
	{
		g_node_to_node_shorest_travel_time[origin_node][i] = g_node_label_earliest_arrival_time[i];

	}
	return total_cost;

}

#pragma endregion

#pragma region <step 1 & step 3 :  prepare + dataOut>

void g_generate_travel_time_matrix()
{
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
	for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
	{
		g_accessibility_matrix[p1][p2] = 1;
	}

	if(g_debug_out)fprintf(g_pFileDebugLog, "--- travel time and cancelation cost ($)----\n");

	// for each pax
	for (int p = 1; p <= g_number_of_passengers; p++)
	{ 
		//cout << ">>find shortest path tree for pax p = " << p << "..." << endl;
	
		g_optimal_label_correcting(g_passenger_origin_node[p], g_passenger_departure_time_beginning[p]);//fulfill 
		
		float earliest_arrival_time = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p]][g_passenger_destination_node[p]];
		
		if (earliest_arrival_time>1440)
		{
			cout << "Abnormal earliest arrival time. Program stops." << endl;
			g_ProgramStop();
		}

		g_passenger_request_travel_time_vector[p] = max(5, earliest_arrival_time - g_passenger_departure_time_beginning[p]);

		g_passenger_request_cancelation_cost[p] = g_passenger_request_travel_time_vector[p] / 60.0 * 30;
		// from the pax's destination to all the other nodes starting from the earliest arrival time at the d
		g_optimal_label_correcting(g_passenger_destination_node[p], earliest_arrival_time);		
		if(g_debug_out)fprintf(g_pFileDebugLog, "pax no.%d, Departure Time = %d (min), Travel Time = %.2f (min), Earliest Arrival Time = %.2f, cost = $%.2f\n",
			p, g_passenger_departure_time_beginning[p],
			g_passenger_request_travel_time_vector[p],
			earliest_arrival_time,
			g_passenger_request_cancelation_cost[p]);

		if (g_passenger_arrival_time_beginning[p] < earliest_arrival_time + 2)
		{
			int existing_arrival_time_window = g_passenger_arrival_time_ending[p] - g_passenger_arrival_time_beginning[p];
			
			fprintf(g_pFileDebugLog, "modified arrival time window for pax no.%d [%d,%d]  \n",
				p, earliest_arrival_time + 2, earliest_arrival_time + 2 + existing_arrival_time_window);

			g_passenger_arrival_time_beginning[p] = earliest_arrival_time + 2;
			g_passenger_arrival_time_ending[p] = g_passenger_arrival_time_beginning[p] + existing_arrival_time_window; 

			float ideal_profit = g_passenger_request_travel_time_vector[p] / 60 * 22;  // 22 dolloar per 60 min 

			if (g_passenger_base_profit[p] < ideal_profit)
			{
				fprintf(g_pFileDebugLog, "modified base profit for pax no.%d from %f to %f  \n",
					p, g_passenger_base_profit[p], ideal_profit);

				g_passenger_base_profit[p] = ideal_profit;
			}


		}

	}
	//check rule 1:
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
	for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
	{
		float departure_time_difference = abs(g_passenger_departure_time_beginning[p1] - g_passenger_departure_time_beginning[p2]);
		float minimum_travel_time_p1_p2 = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p1]][g_passenger_origin_node[p2]];
		float minimum_travel_time_p2_p1 = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p2]][g_passenger_origin_node[p1]];

		if (departure_time_difference < min(minimum_travel_time_p1_p2, minimum_travel_time_p2_p1))
		{
			g_accessibility_matrix[p1][p2] = 0;
		}
	}

	// we now have g_node_to_node_shorest_travel_time from all activity node
	fprintf(g_pFileOutputLog, "Least Travel Time for Pax,");
	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileOutputLog, "Pax %d,", p);

	}
	fprintf(g_pFileOutputLog, "\n,");

	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileOutputLog, "%.2f,", g_passenger_request_travel_time_vector[p]);

	}
	fprintf(g_pFileOutputLog, "\n");

	// calculate shortest path from each vehicle's origin to all nodes

	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		//cout << ">>find shortest path tree for vehicle v = " << v << "..." << endl;
		g_optimal_label_correcting(g_vehicle_origin_node[v], g_vehicle_departure_time_beginning[v]);

		for (int i = 0; i < g_number_of_nodes; i++) //Initialization for all nodes
		{
			g_vehicle_origin_based_node_travel_time[v][i] = max(1, g_node_label_earliest_arrival_time[i] - g_vehicle_departure_time_beginning[v]);		
		//	fprintf(g_pFileOutputLog, "V %d DN %d =,%f\n", v, i, g_vehicle_origin_based_node_travel_time[v][i]);		
		}
		
	}
	// calculate shortest path from each vehicle's destination to all nodes

	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		//cout << ">>find shortest path tree for vehicle v = " << v << "..." << endl;
		g_optimal_label_correcting(g_vehicle_destination_node[v], 0);

		for (int i = 0; i < g_number_of_nodes; i++) //Initialization for all nodes
		{
			g_vehicle_destination_based_node_travel_time[v][i] = max(1, g_node_label_earliest_arrival_time[i]);
		//	fprintf(g_pFileOutputLog, "V %d ON %d =,%f\n", v, i, g_vehicle_destination_based_node_travel_time[v][i]);

		}

	}

	fprintf(g_pFileOutputLog, "Reduced Search Space Perc for Veh,");
	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		fprintf(g_pFileOutputLog, "Veh %d,", v);

	}
	fprintf(g_pFileOutputLog, "\n,");


	// calculate the % of nodes can be skipped 
	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		int number_of_nodes_to_be_skipped = 0;
		int time_window_length = max(10, g_vehicle_arrival_time_ending[v] - g_vehicle_departure_time_ending[v]);

		for (int i = 1; i <= g_number_of_nodes; i++)  // for each node
		{

			float travel_time_3_points = g_vehicle_origin_based_node_travel_time[v][i] + g_vehicle_destination_based_node_travel_time[v][i];

			// if the total travel time from origin to node i then back to destination is greater than the time window, then skip this node/link to scan
		    if (travel_time_3_points >= time_window_length)
					number_of_nodes_to_be_skipped++;

		}
		fprintf(g_pFileOutputLog, "%.2f%%,", number_of_nodes_to_be_skipped*100.0 / max(1, g_number_of_nodes));

	}
	fprintf(g_pFileOutputLog, "\n,");

	
}

void g_get_group_vp_from_trans_data()
{	
	CCSVParser parser;
	if (parser.OpenCSVFile("trans_B_partition_g_v_p.csv", true))
	{
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			
			int group_number;
			int vehicle_number;
			int passenger_number;
			parser.GetValueByFieldName("g", group_number);
			parser.GetValueByFieldName("v", vehicle_number);
			parser.GetValueByFieldName("p", passenger_number);

			if (passenger_number <= 0) /////for g_group_v
			{
				g_group_v[group_number][vehicle_number] = 1; g_group_v[group_number][0]++;
			}
			else if (vehicle_number <= 0)///for g_group_p
			{
				g_group_p[group_number][passenger_number] = 1; g_group_p[group_number][0] ++;
			}
			g_total_group_number = group_number;
		}
		parser.CloseCSVFile();

		////////for virtual vehicle 
		for (int g = 1; g <= g_total_group_number; g++)
		{
			g_group_v[g][g_number_of_physical_vehicles + 1] = 1;///test only one virtual vehicle
		}


		/////////print
		fprintf(g_pFileOutputLog, "\n group,");
		for (int v_in = 1; v_in <= g_number_of_physical_vehicles; v_in++)
		{
			fprintf(g_pFileOutputLog, "v%d,", v_in);
		}
		for (int g = 1; g <= g_total_group_number; g++)
		{
			fprintf(g_pFileOutputLog, "\ng-%d,", g);
			for (int v_in = 1; v_in <= g_number_of_physical_vehicles; v_in++)
			{
				fprintf(g_pFileOutputLog, "%d,", g_group_v[g][v_in]);
			}
		}

		fprintf(g_pFileOutputLog, "\n group,");
		for (int p_in = 1; p_in <= g_number_of_passengers; p_in++)
		{
			fprintf(g_pFileOutputLog, "p%d,", p_in);
		}
		for (int g = 1; g <= g_total_group_number; g++)
		{
			fprintf(g_pFileOutputLog, "\ng-%d,", g);
			for (int p_in = 1; p_in <= g_number_of_passengers; p_in++)
			{
				fprintf(g_pFileOutputLog, "%d,", g_group_p[g][p_in]);
			}
		}
	}
}


void g_partition_group_network()
{
	for (int g = 1; g <= g_total_group_number; g++)
	{
		/*int groupNodes[_MAX_NUMBER_OF_NODES] = {0};*/
		///////1:p_o->p_d (include dummy nodes and links)
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
			if (g_group_p[g][p] == 0)continue;

			int from_node = g_passenger_origin_node[p];
			int to_node = g_passenger_destination_node[p];

			g_group_node[g][from_node] = 1;
			int dummy_from_node = g_number_of_physical_nodes + (p - 1) * 2;
			g_group_node[g][dummy_from_node] = 1;

			int dummy_link1 = g_fromNode_ToNode_map_linkID[connect_2_int(from_node, dummy_from_node)];
			int dummy_link2 = g_fromNode_ToNode_map_linkID[connect_2_int(dummy_from_node, from_node)];
			g_group_link[g][dummy_link1] = 1;///add link
			g_group_link[g][dummy_link2] = 1;///add link
			

			g_group_node[g][to_node] = 1;
			int dummy_destination_node = g_number_of_physical_nodes + (p - 1) * 2 + 1;
			g_group_node[g][dummy_destination_node]=1;

			dummy_link1 = g_fromNode_ToNode_map_linkID[connect_2_int(to_node, dummy_destination_node)];
			dummy_link2 = g_fromNode_ToNode_map_linkID[connect_2_int(dummy_destination_node, to_node)];
			g_group_link[g][dummy_link1] = 1;///add link
			g_group_link[g][dummy_link2] = 1;///add link
			
			g_optimal_label_correcting(from_node, g_passenger_departure_time_beginning[p]);//fulfill 
			int predecessor_node = to_node;
			while (predecessor_node != -1)
			{
				int now_node = g_node_static_predecessor[predecessor_node];
				if (now_node == -1)break;

				string str_node = connect_2_int(now_node, predecessor_node);
				int now_link = g_fromNode_ToNode_map_linkID[str_node];

				g_group_node[g][now_node] = 1;////add node
				g_group_link[g][now_link] = 1;///add link				
				predecessor_node = now_node;
			}			

		}
		
		///////2.1:p_d->p_o 
		for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		{
			if (g_group_p[g][p1] == 0)continue;
			for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
			{
				if (g_group_p[g][p2] == 0)continue;
				if (p2 == p1)continue;
							
				int from_node = g_passenger_destination_node[p1];
				int to_node = g_passenger_origin_node[p2];

				g_optimal_label_correcting(from_node,g_passenger_arrival_time_beginning[p1]);//fulfill 
				int predecessor_node = to_node;
				while (predecessor_node != -1)
				{
					int now_node = g_node_static_predecessor[predecessor_node];
					if (now_node == -1)break;

					string str_node = connect_2_int(now_node, predecessor_node);
					int now_link = g_fromNode_ToNode_map_linkID[str_node];

					g_group_node[g][now_node] = 1;////add node
					g_group_link[g][now_link] = 1;///add link				
					predecessor_node = now_node;
				}
			}
		}
		///////2.2:p_o->p_o 
		for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		{
			if (g_group_p[g][p1] == 0)continue;
			for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
			{
				if (g_group_p[g][p2] == 0)continue;
				if (p2 == p1)continue;

				int from_node = g_passenger_origin_node[p1];
				int to_node = g_passenger_origin_node[p2];

				g_optimal_label_correcting(from_node, g_passenger_departure_time_beginning[p1]);//fulfill 
				int predecessor_node = to_node;
				while (predecessor_node != -1)
				{
					int now_node = g_node_static_predecessor[predecessor_node];
					if (now_node == -1)break;

					string str_node = connect_2_int(now_node, predecessor_node);
					int now_link = g_fromNode_ToNode_map_linkID[str_node];

					g_group_node[g][now_node] = 1;////add node
					g_group_link[g][now_link] = 1;///add link				
					predecessor_node = now_node;
				}
			}
		}
		///////2.3:p_d->p_d 
		for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		{
			if (g_group_p[g][p1] == 0)continue;
			for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
			{
				if (g_group_p[g][p2] == 0)continue;
				if (p2 == p1)continue;

				int from_node = g_passenger_destination_node[p1];
				int to_node = g_passenger_destination_node[p2];

				g_optimal_label_correcting(from_node, g_passenger_arrival_time_beginning[p1]);//fulfill 
				int predecessor_node = to_node;
				while (predecessor_node != -1)
				{
					int now_node = g_node_static_predecessor[predecessor_node];
					if (now_node == -1)break;

					string str_node = connect_2_int(now_node, predecessor_node);
					int now_link = g_fromNode_ToNode_map_linkID[str_node];

					g_group_node[g][now_node] = 1;////add node
					g_group_link[g][now_link] = 1;///add link				
					predecessor_node = now_node;
				}
			}
		}




		///////3:v_o->p_o 
		for (int v = 1; v <= g_number_of_physical_vehicles; v++)
		{
			if (g_group_v[g][v] == 0)continue;
			for (int p = 1; p <= g_number_of_passengers; p++)
			{
				if (g_group_p[g][p] == 0)continue;

				int from_node =g_vehicle_origin_node[v];
				int to_node = g_passenger_origin_node[p];

				g_optimal_label_correcting(from_node, g_vehicle_departure_time_beginning[v]);//fulfill 
				int predecessor_node = to_node;
				while (predecessor_node != -1)
				{
					int now_node = g_node_static_predecessor[predecessor_node];
					if (now_node == -1)break;

					string str_node = connect_2_int(now_node, predecessor_node);
					int now_link = g_fromNode_ToNode_map_linkID[str_node];

					g_group_node[g][now_node] = 1;////add node
					g_group_link[g][now_link] = 1;///add link				
					predecessor_node = now_node;
				}

			}
		}

		///////4:p_d->v_d 
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
			if (g_group_p[g][p] == 0)continue;
			for (int v = 1; v <= g_number_of_physical_vehicles; v++)
			{
				if (g_group_v[g][v] == 0)continue;

				int from_node =g_passenger_destination_node[p];
				int to_node =g_vehicle_destination_node[v];

				g_optimal_label_correcting(from_node,g_passenger_arrival_time_beginning[p]);//fulfill 
				int predecessor_node = to_node;
				while (predecessor_node != -1)
				{
					int now_node = g_node_static_predecessor[predecessor_node];
					if (now_node == -1)break;

					string str_node = connect_2_int(now_node, predecessor_node);
					int now_link = g_fromNode_ToNode_map_linkID[str_node];

					g_group_node[g][now_node] = 1;////add node
					g_group_link[g][now_link] = 1;///add link				
					predecessor_node = now_node;
				}
			}
		}
		//////////////////
		
		for (int n = 0; n <= g_number_of_nodes; n++)
		{
			if (g_group_node[g][n] == 1)
			{
				int group_node_id = ++g_map_groupNode_to_globalNode[g][0];
				g_map_groupNode_to_globalNode[g][group_node_id] = n;
				g_map_globalNode_to_groupNode[g][n] = group_node_id;
			}
		}
		g_map_groupNode_to_globalNode[0][0] = max(g_map_groupNode_to_globalNode[0][0], g_map_groupNode_to_globalNode[g][0]);
		
		
	}
}



void g_generate_file_for_main_programe()
{
	int max_s_number = 0;
	for (int gg = 1; gg <= g_total_group_number; gg++)
		if (g_parameter_c[gg][0] >= max_s_number)max_s_number = g_parameter_c[gg][0];	
	fprintf(g_pFileOutputForMainProgram, "%d,%d,%d,%d\n", g_total_group_number, g_number_of_passengers, max_s_number, -1);

	/////begin c(g,s)
	for (int gg = 1; gg <= g_total_group_number; gg++)
	for (int ss = 1; ss <= max_s_number; ss++)
	{
		float cost = 9999;
		if (ss <= g_parameter_c[gg][0])cost = g_parameter_c[gg][ss];
		fprintf(g_pFileOutputForMainProgram, "%d,%d,%d,%f\n", gg,-1, ss, cost);
	}
	/////begin a(g,p,s)
	for (int gg = 1; gg <= g_total_group_number; gg++)
	for (int pp = 1; pp <= g_number_of_passengers; pp++)
	for (int ss = 1; ss <= max_s_number; ss++)
	{
		if (ss <= g_parameter_a[gg][0][0] && g_parameter_a[gg][pp][ss] == 1)
			fprintf(g_pFileOutputForMainProgram, "%d,%d,%d,%d\n", gg, pp, ss, 1);
	}
	
	
}


void g_create_file_for_vehicle_conncection()
{
	FILE* pFile_vehicle_connection_value = NULL;
	pFile_vehicle_connection_value = fopen("trans_C_vehicle_connection_value.csv", "w");
	if (pFile_vehicle_connection_value == NULL)
	{
		cout << "File trans_C_vehicle_connection_value.txt cannot be opened." << endl;
		g_ProgramStop();
	}
	fprintf(pFile_vehicle_connection_value, "u,v,cost,travelTime\n"); // header


	FILE* pFile_vehicle_connection_route = NULL;
	pFile_vehicle_connection_route = fopen("trans_C_vehicle_connection_route.csv", "w");
	if (pFile_vehicle_connection_route == NULL)
	{
		cout << "File trans_C_vehicle_connection_route.txt cannot be opened." << endl;
		g_ProgramStop();
	}
	fprintf(pFile_vehicle_connection_route, "u,v,p1,p2,time,node,cost\n"); // header

	int active_u_list[_MAX_NUMBER_OF_VEHICLES];
	int cost_uv_connection[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_VEHICLES] = {0};
	/////////////////

	for (int u = 1; u <= g_number_of_physical_vehicles; u++)
	{
		if (g_vehicle_latest_deliver_passenger[u] == 0)continue;
		for (int v = 1; v <= g_number_of_physical_vehicles; v++)
		{
			if (g_vehicle_earliest_pickup_passenger[v] == 0)continue;

			int group_u, group_v;
			for (int g = 1; g <= g_total_group_number; g++)
			{
				if (g_group_v[g][u] == 1)group_u = g;
				if (g_group_v[g][v] == 1)group_v = g;
			}
			if (group_u == group_v)continue;
			////////////
			active_u_list[u] = 1;

			int u_p1=g_vehicle_latest_deliver_passenger[u];			
			int v_p2 = g_vehicle_earliest_pickup_passenger[v];
			int u_time1 = g_vehicle_latest_deliver_time[u];
			int v_time2 = g_vehicle_earliest_pickup_time[v];

			int from_node = g_passenger_destination_node[u_p1];
			int to_node = g_passenger_origin_node[v_p2];

			g_optimal_label_correcting(from_node, u_time1);//fulfill 			

			if (g_node_label_earliest_arrival_time[to_node] > v_time2) continue;////it means unreachable

			cost_uv_connection[u][v] = v_time2;
			fprintf(pFile_vehicle_connection_value, "%d,%d,%d,%d\n", u, v, cost_uv_connection[u][v], (int)g_node_label_earliest_arrival_time[to_node] - u_time1);

			//////print connection node
			
			int reversed_path_node_sequence[_MAX_NUMBER_OF_NODES];			
			int	node_size = 0;
			reversed_path_node_sequence[node_size] = to_node;
			node_size++;
			int pred_node = to_node;
			while (pred_node != -1)
			{			

				int now_node = g_node_static_predecessor[pred_node];
				if (now_node == -1)break;			
				reversed_path_node_sequence[node_size] = now_node;	
				pred_node = now_node;
				node_size++;	

			}

			for (int n = 0; n < node_size; n++)
			{
				int now_path_node = reversed_path_node_sequence[node_size - n - 1];
				int now_path_time = g_node_label_earliest_arrival_time[now_path_node];
				///////////
				fprintf(pFile_vehicle_connection_route, "%d,%d,%d,%d,%d,%d,%d\n", u, v, u_p1, v_p2, now_path_time,g_external_node_id_map[now_path_node], now_path_time - u_time1);
			}




		}
	}
	////////////////
	///////print pFile_vehicle_connection_value
	//stringstream str_one_line;
	//str_one_line << "set u /";
	//int last_u = 0;
	//for (int u = g_number_of_physical_vehicles; u >= 1; u--)
	//{
	//	if (active_u_list[u] == 1){
	//		last_u = u; break;
	//	}
	//}
	//for (int u = 1; u <= g_number_of_physical_vehicles; u++)
	//{		
	//	if (active_u_list[u] == 1)str_one_line << int2str(u);
	//	if (u != last_u) str_one_line << ",";
	//}
	//str_one_line << "/;\n";

	//fprintf(pFile_vehicle_connection_value, str_one_line.str().c_str());

	//str_one_line.str("");
	//str_one_line << "scalar M /1000/;\n";
	//fprintf(pFile_vehicle_connection_value, str_one_line.str().c_str());

	//str_one_line.str("");
	//str_one_line << "scalar Infinite /99999/;\n";
	//fprintf(pFile_vehicle_connection_value, str_one_line.str().c_str());

	//str_one_line.str("");
	//str_one_line << "parameter c(u,v)/\n";
	//for (int u = 1; u <= g_number_of_physical_vehicles; u++)
	//{
	//	for (int v = 1; v <= g_number_of_physical_vehicles; v++)
	//	{
	//		if (active_u_list[u] != 1)continue;
	//		if (active_u_list[v] != 1)continue;
	//		str_one_line << int2str(u) << "." << int2str(v) << "  ";
	//		
	//		int real_value;
	//		if (cost_uv_connection[u][v] != 0)
	//			real_value = cost_uv_connection[u][v];
	//		else
	//			real_value=99999;
	//		if (u == v)
	//			real_value = 1000;

	//		str_one_line << int2str(real_value) << "\n";
	//	}
	//}

	//str_one_line << "/;\n";	
	//fprintf(pFile_vehicle_connection_value, str_one_line.str().c_str());

	fclose(pFile_vehicle_connection_value);
	fclose(pFile_vehicle_connection_route);
	

}

#pragma endregion

#pragma region <step 2 : main function entrance>

void g_Fulfill_ArcTravel_And_Node_Waiting_Cost(float VOIVTT_per_hour)
{

	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		// setup arc travelling cost
		for (int link = 0; link < g_number_of_links; link++)
		{
			for (int t = 0; t < g_number_of_time_intervals; t++)
			{
				g_v_arc_cost[v][link][t] = g_arc_travel_time[link][t] / 60.0 * VOIVTT_per_hour;  // 60 min per hour

				if (g_arc_travel_time[link][t]>0)
				{
					int q = 0;
				}
			}
		}

		// setup waiting cost
		for (int node = 0; node <= g_number_of_nodes; node++)
		{
			for (int t = 0; t <= g_number_of_time_intervals; t++)
			{
				g_v_vertex_waiting_cost[v][node][t] = 0;
			}
		}


	}

}

void g_init_costs_for_main_function()
{
	fprintf(g_pFileOutputLog, "\n");


	cout << "Preparation......" << endl;
	int VOIVTT_per_hour = 50;
	g_Fulfill_ArcTravel_And_Node_Waiting_Cost(VOIVTT_per_hour);//convert link travel time to arc travelling cost, and set all node waiting cost to be 0

	//step 0: initialization 
	if (g_debug_out)fprintf(g_pFileDebugLog, "step 0: initialization \n");


	if (_MAX_NUMBER_OF_LINKS < g_number_of_links)
	{

		cout << "Number of links = " << g_number_of_links << ", which is greater then the max threshold of " << _MAX_NUMBER_OF_LINKS;
		g_ProgramStop();
	}
	for (int link = 0; link < g_number_of_links; link++)
	{
		for (int t = 0; t < g_number_of_time_intervals; t++)
		{
			g_arc_travel_time[link][t] = g_link_free_flow_travel_time[link];  //transportation cost
		}
	}

	// setup waiting cost
	for (int node = 0; node <= g_number_of_nodes; node++)
	{
		for (int t = 0; t <= g_number_of_time_intervals; t++)
		{
			for (int v = 1; v <= g_number_of_vehicles; v++)//note that the scheduling sequence does not matter  here
			{
				g_v_vertex_waiting_cost[v][node][t] = 1;
				g_vertex_visit_count_for_lower_bound[v][node][t] = 0;
			}
		}
	}


	//cout << "Start scheduling passengers by Lagrangian Relaxation method" << endl;
	//cout << "Running Time:" << g_GetAppRunningTime() << endl;
	//g_SolutionStartTime = CTime::GetCurrentTime();

	for (int v = 1; v <= g_number_of_vehicles; v++)
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
		g_vehicle_serving_passenger_matrix[v][p] = 0;

		}



	//step 2: shortest path for vehicle

	for (int v = 1; v <= g_number_of_vehicles; v++)//note that the scheduling sequence does not matter  here  // include both physical and virtual vehicles
	{

		// set arc cost, to_node_cost and waiting_cost for vehicles

		for (int link = 0; link < g_number_of_links; link++)
		{
			for (int t = 0; t < g_number_of_time_intervals; t++)
			{
				g_v_arc_cost[v][link][t] = g_arc_travel_time[link][t] / 60.0 * g_VOIVTT_per_hour[v];  // 60 min pur hour
			}
		}

		// setup waiting cost
		for (int node = 0; node <= g_number_of_nodes; node++)
		{
			for (int t = 0; t <= g_number_of_time_intervals; t++)
			{

				g_v_vertex_waiting_cost[v][node][t] = 1 / 60.0* g_VOWT_per_hour[v];
			}
		}
		// special case: no waiting cost at vehicle returning depot

		for (int t = 0; t <= g_number_of_time_intervals; t++)
		{
			int vehicle_destination_node = g_vehicle_destination_node[v];
			g_v_vertex_waiting_cost[v][vehicle_destination_node][t] = 0;
		}

	}
}

bool g_Multi_VRP_Solver()
{

	g_init_costs_for_main_function();///init

	////for each group: start a m_vrp
	for (int g = 1; g <= g_total_group_number; g++)
	{
		cout << "start group:" << g << " /" << g_total_group_number;

		//if (g_debug_out)
		{
			fprintf(g_pFileDebugLog, "\n **********************  Start Group = %d ***************************\n\n", g);
			fprintf(g_pFileDebugLog, "g(v)=");
			for (int v = 1; v <=g_number_of_physical_vehicles; v++)
			{
				if (g_group_v[g][v] == 1)
				{
					if (v > g_number_of_physical_vehicles)fprintf(g_pFileDebugLog, "*%d + ", v);
					else fprintf(g_pFileDebugLog, "%d + ", v);
				}
			}
			fprintf(g_pFileDebugLog, "\ng(p)=");
			for (int p = 1; p <= g_number_of_passengers; p++)
			{
				if (g_group_p[g][p] == 1)
				{
					fprintf(g_pFileDebugLog, "%d + ", p);
				}
			}
			fprintf(g_pFileDebugLog, "\n");
		}
		/////////////////////////////////
		for (int ii = 0; ii < _MAX_NUMBER_OF_VEHICLES; ii++)
		{
			g_DynamicStateVector[ii].clear();
			g_service_state_map[ii].clear();
		}

		int v_start = 0;
		for (int v = 1; v <= g_number_of_physical_vehicles; v++)
		{
			if (g_group_v[1][v] == 1)v_start = v;
		}
		cout << "\n    create stage 1 service states....";
		g_create_stage_service_states(g, 1,v_start, g_number_of_passengers);
		cout << "\n    stage 1, total states number:" << g_DynamicStateVector[1].size() << "\n"<<endl;

		

		g_print_stage_states(1);/////print stage's states.

		g_parameter_c[g][0] = 0;///init parameter
		g_parameter_a[g][0][0] = 0;

		g_integrated_assignment_routing_dynamic_programming(g);

	}
	return true;
}

void g_ProgramStop()
{

	cout << "Agent+ Program stops. Press any key to terminate. Thanks!" << endl;
	getchar();
	exit(0);
};

#pragma endregion


#pragma region <_tmain>

int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;
	HMODULE hModule = ::GetModuleHandle(NULL);

#pragma region  [    file inital]
	if (hModule != NULL)
	{
		// initialize MFC and print and error on failure
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			// TODO: change error code to suit your needs
			_tprintf(_T("Fatal Error: MFC initialization failed\n"));
			nRetCode = 1;
		}
		else
		{
			// TODO: code your application's behavior here.
		}
	}
	else
	{
		// TODO: change error code to suit your needs
		_tprintf(_T("Fatal Error: GetModuleHandle failed\n"));
		nRetCode = 1;
	}

	g_pFileDebugLog = fopen("Debug.txt", "w");
	if (g_pFileDebugLog == NULL)
	{ 
		cout << "File Debug.txt cannot be opened." << endl;
		g_ProgramStop();
	}

	
	g_pFileOutputForMainProgram = fopen("trans_C_group_state_parameter.csv", "w");
	if (g_pFileOutputForMainProgram == NULL)
	{
		cout << "File trans_C_group_state_parameter.csv cannot be opened." << endl;
		g_ProgramStop();
	}

	g_pFile_All_Routes = fopen("trans_C_file_all_routes.csv", "w");
	if (g_pFile_All_Routes == NULL)
	{
		cout << "File trans_C_file_all_routes.csv cannot be opened." << endl;
		g_ProgramStop();
	}

	 g_pFileOutputLog = fopen("output_information_record.csv", "w");
	 if (g_pFileOutputLog == NULL)
	 {
		 cout << "File output_information_record.csv cannot be opened." << endl;
		 g_ProgramStop();
	 }
	 g_pFile_Map_GS_Str = fopen("trans_C_map_gs_str.csv", "w");
	 if (g_pFile_Map_GS_Str == NULL)
	 {
		 cout << "File trans_C_map_gs_str.csv cannot be opened." << endl;
		 g_ProgramStop();
	 }
	 
	fprintf(g_pFile_Map_GS_Str, "g,s_index,state_str,p\n"); // header
	fprintf(g_pFileOutputForMainProgram, "g,p,s,value\n"); // header
	fprintf(g_pFile_All_Routes, "g,s_index,v,p,time,node,cost,route_type\n"); // header

#pragma endregion

	// define timestamps
	clock_t start_t, end_t, total_t;
	start_t = clock();
	

	g_ReadConfiguration();	
	g_ReadInputData();

	
	g_allocate_memory_travel_time(0);//allocate memory	
	
	g_get_group_vp_from_trans_data();//////////step 1.1: get g_group_v[g][v] & g_group_p[g][p]
	
	g_generate_travel_time_matrix();      ////step 1.2: init shortest path time  && get node_sub[]，implem
	
	g_partition_group_network();////step1.3: partion network into group


	g_allocate_memory_DP(0);///allocate memory		
     
	g_Multi_VRP_Solver();  ///////////////////main step 2: call function foreach Group generate routes

	g_create_file_for_vehicle_conncection();


#pragma region [    data out & file close]
	/////////////generate txt file for gams
	//cout << "generate txt file for gams....\n"<<endl;
	//g_generate_file_for_gams();
	
	///////////
	cout << "generate txt file for main_programe in c#....\n";
	g_generate_file_for_main_programe();
	/////////////////computing time cosuming
	end_t = clock();
	total_t = (end_t - start_t);	
	cout << "\nCPU Running Time = " << total_t << " milliseconds" << endl;
	fprintf(g_pFileDebugLog, "\n-------------------------------------------------------");
	fprintf(g_pFileDebugLog, "\nCPU Running Time = %ld milliseconds\n", total_t);
	fprintf(g_pFileOutputLog, "CPU Running Time =,%ld, milliseconds\n", total_t);
	

	fclose(g_pFileOutputLog);
	fclose(g_pFileOutputForMainProgram);
	fclose(g_pFileDebugLog);
	fclose(g_pFile_All_Routes);
	fclose(g_pFile_Map_GS_Str);
	

	cout << "End of Optimization " << endl;	
	cout << "free memory.." << endl;
	
	

#pragma endregion
	
	g_free_memory_DP(0);

	g_free_memory_travel_time(0);
	
	cout << "done." << endl;
	return nRetCode;
}

#pragma endregion


